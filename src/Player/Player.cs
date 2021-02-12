using Godot;
using System;
using System.Collections.Generic;
using EvilFarmingGame.Items;
using EvilFarmingGame.Player;
using EvilFarmingGame.Tiles;

public class Player : KinematicBody2D
{
    public PlayerController Controller;
    public Inventory Inventory;
    private AnimatedSprite Sprite;

    public float Currency = 0;

    public bool CanMove = true;
    public bool IsDragging = false;
    public KinematicBody2D DragginBody;
    
    public Node2D RayPivot;
    public Area2D CollidingInteractable;
    public Vector2 Velocity;

    public Control UI;
    
    private RichTextLabel Clock;
    public DayNight TimeNode;

    private RichTextLabel MessageLabel;
    private Dialogue DialogueBox;
    private RichTextLabel CurrencyLabel;
    
    // --- Player Stats ---
    
    // Stamina 
    [Export()] public float StaminaDecreaseFactor = 20f;
    [Export()] public float StaminaIncreaseFactor = .2f;
    [Export()] public float Stamina = 100;
    [Export()] public float MaxStamina = 100;
    
    
    public override void _Ready()
    {
        // Fetch all the nodes
        Controller = new PlayerController(this);
        Sprite = (AnimatedSprite) GetNode("Sprite");

        RayPivot = (Area2D) GetNode("RayPivot");

        UI = (Control) GetNode("UI/ControlUI");
        
        Clock = (RichTextLabel) GetNode("UI/ControlUI/Clock");
        DialogueBox = (Dialogue) GetNode("DialogueBox");

        CurrencyLabel = (RichTextLabel) GetNode("UI/ControlUI/Currency");
        Currency += 5;
        
        Inventory = new Inventory(5); // Instantiate the inventory
        
        // Handles the time node
        if(Owner.HasNode("DayNight"))
            TimeNode = (DayNight) Owner.GetNode("DayNight");
        else
            GD.Print("WARNING: No DayNight Node, day-night system won't be implemented in scene (Crops won't grow, daylight amount won't change, etc).");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (CanMove)
        {
            Velocity = Controller.InputMovement(delta);
            AnimationHandeling();
        }
        
        InventoryHandling();
        PlayerLogic();
        StatsHandling();
        RayCasting();
        if (TimeNode != null) TimeHandling();
    }

    public override void _Input(InputEvent @event)
    {
        // Dropping Items
        if (Input.IsActionJustPressed("Player_Drop") && Inventory.HeldSlot < Inventory.Slots.Count && CanMove)
        {
            var Item = (ItemEntity) ((PackedScene)GD.Load("res://src/Items/ItemEntity.tscn")).Instance(); // Creates a new ItemEntity instance
            
            Item.CurrentItem = Inventory[Inventory.HeldSlot]; // Sets the ItemEntity's item
            Item.Position = Position; // Sets the ItemEntity's position
            Inventory.Remove(Inventory[Inventory.HeldSlot]); // Removes the item from the players inventory
            Item.IsJustDropped = true;

            GetParent().GetNode("Items").AddChild(Item); // Drops the item
        }

        if (Input.IsActionJustPressed("Player_UseItem") && CanMove)
        {
            // Handles Item interaction
            if (Inventory.HeldSlot >= Inventory.Slots.Count) return; // returns if the selected slot doesnt have an item
            
            // Handles the item-interaction
            switch (Inventory[Inventory.HeldSlot].item)
            {
                // Use the tool
                case Tool T:
                    T.Use(this); // Uses the tool
                    break;
                
                // Eat the crop if it is edible
                case Crop T:
                    if (T.IsEdible) T.Eat(this); // Eats the crop if it is edible
                    break;
                
                // Plant the seed
                case Seed T:
                    if (!(CollidingInteractable is FarmLand farmLand)) break; // Checks if the player is interacting with farmland
                    farmLand.AddSeed(T); // Adds the seed to the farmland
                    Inventory.Remove(Inventory[Inventory.HeldSlot].item); // Removes the seed from the players inventory 
                    break;
                    
                // Place the placeable-item
                case PlaceableItem T:
                    var itemBody = (PlacedItem) ((PackedScene)GD.Load(T.ScenePath)).Instance(); // Create new PlacedItem instance
                    itemBody.CurrentItem = T; // Set the item of the placed item
                    itemBody.Position = Position; // Set the position of the placed item
            
                    Inventory.Remove(T); // Remove the item from the inventory
                    GetParent().GetNode("Environment/PlacedItems").AddChild(itemBody); // Add the PlacedItem instance to the scene
                    break;
            }
        }

        if (!Input.IsActionJustPressed("Player_Action") || !CanMove) return; // Checks if the player action is pressed and if the player can interact
        {
            if (CollidingInteractable == null) return; // Checks if the CollidingTile is null
            switch (CollidingInteractable) // Checks the colliding tile type
            {
                // Interacts with interactable tile
                case InteractableTile T:
                    T.Interact(this);
                    break;
            }
        }

    }
    
    // Handles player animation
    private void AnimationHandeling()
    {
        switch (Controller.CurrentDirection)
        {
            case PlayerController.Direction.Left:
                if (Velocity.x != 0 || Velocity.y != 0)
                {
                    Sprite.Play("Left-walk");
                    break;
                }
                Sprite.Play("Left");
                break;
            case PlayerController.Direction.Right:
                if (Velocity.x != 0 || Velocity.y != 0)
                {
                    Sprite.Play("Right-walk");
                    break;
                }
                Sprite.Play("Right");
                break;
            case PlayerController.Direction.Up:
                if (Velocity.x != 0 || Velocity.y != 0)
                {
                    Sprite.Play("Up-walk");
                    break;
                }
                Sprite.Play("Up");
                break;
            case PlayerController.Direction.Down:
                if (Velocity.x != 0 || Velocity.y != 0)
                {
                    Sprite.Play("Down-walk");
                    break;
                }
                Sprite.Play("Down");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void InventoryHandling()
    {
        for (int i = 0; i < Inventory.Slots.Count; i++)
        {
            var icon = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Item"); // Fetch the needed ui inventory slot
            var amountLabel = (RichTextLabel) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/AmountLabel"); // Fetch the amount-label
            if (Inventory[i].item == null) continue; // Continues the loop if the slot has no item 
            
            icon.Texture = Inventory[i].item.Icon; // Applies the item's icon to the inventory ui
            amountLabel.BbcodeText = Inventory[i].item.MaxStackAmount > 1 // Sets the amount-label to the correct amount
                ? Inventory[i].Amount.ToString() 
                : "";
        }

        for (int i = 0; i < Inventory.Slots.Capacity; i++)
        {
            var icon = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Item"); // Fetch the needed ui inventory slot
            var selectBox = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Selection"); // Fetch the select-box
            var amountLabel = (RichTextLabel) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/AmountLabel"); // Fetch the amount-label

            selectBox.Visible = (i == Inventory.HeldSlot); // Makes the select-box visible if the slot is selected

            if (i < Inventory.Slots.Count) continue; // continues the loop if a slot is not empty
            icon.Texture = null; // clears the slot-icon
            amountLabel.BbcodeText = ""; // clears the amount-label
        }
        
        // Changes the selected slot
        if (Input.IsActionJustPressed("ui_right")) 
        {
            if (Inventory.HeldSlot < Inventory.Slots.Capacity - 1)
                Inventory.HeldSlot++;
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            if (Inventory.HeldSlot > 0)
                Inventory.HeldSlot--;
        }
    }

    // Handles the players interaction with objects
    private void RayCasting()
    {   
        // Automatically picks up items
        foreach (Area2D CollidedArea in GetNode<Area2D>("ItemPickUpRange").GetOverlappingAreas())
        {
            if (!(CollidedArea is ItemEntity it)) continue;
            it.PlayerColliding = true;
            it.PlayerBody = this;
        }
        
        // Interacts with interactable areas
        foreach (Area2D RayCast in GetTree().GetNodesInGroup("PlayerRays"))
        {
            var collided = false; // Checks if the area has and to breaks the loop
            if (RayCast.GetOverlappingAreas().Count <= 0) continue; // Checks if the area has collided
            var collidedTile = RayCast.GetOverlappingAreas()[0]; // Fetches the collided tile
            
            // Checks the tiles type
            switch (collidedTile)
            {
                // Handles interactable tiles
                case InteractableTile T:
                    T.OutLine.Visible = true;
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    CollidingInteractable = T;
                    collided = true;
                    break;
                
                // Handles NPC's
                case NPC T:
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    collided = true;
                    break;
                
                // Handles placed items
                case PlacedItem T:
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    CollidingInteractable = T;
                    collided = true;
                    break;
            }
            
            if (collided) break; // Breaks the loop if collided
            CollidingInteractable = null; // Clears the interactable tile if the area hasn't collided with a tile
        }
    }
    
    // Displays the time
    private void TimeHandling()
    {
        Clock.BbcodeText = TimeNode.Afternoon
            ? $"Day: {TimeNode.Day}, {TimeNode.Hour}:{TimeNode.Minute:00} PM"
            : $"Day: {TimeNode.Day}, {TimeNode.Hour}:{TimeNode.Minute:00} AM";
    }
    
    // Handles the players stats
    private void StatsHandling()
    {
        var statsUI = (Control) UI.GetNode("Stats"); // Fetch the stats ui node
        
        // Stamina
        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina); // Clamps the stamina to a value between 0-MacStamina
        
        var staminaBar = (TextureProgress) statsUI.GetNode("Stamina"); // Fetches the stamina bar
        staminaBar.Value = Stamina; // Sets the value of the stamina-bar to the current value
        staminaBar.MaxValue = MaxStamina; // Sets the max value of the stamina-bar to the current max value
        
        // Currency
        if (CurrencyLabel != null)
            CurrencyLabel.BbcodeText = $"{Currency}G"; // Updates the currency value

    }
    
    // Handles the player logic
    private void PlayerLogic()
    {
        if (!DialogueBox.IsShown && UI.Visible)
            CanMove = true; // Allows the player to move if no UI is shown
    }

    public void MessagePlayer(string Message) // Announces a message to the player
    {
        DialogueBox.PlayerBody = this; 
        if (!DialogueBox.IsShown && CanMove)
            DialogueBox.Announce(Message);
    }
    
}

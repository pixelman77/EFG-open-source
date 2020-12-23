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
        Controller = new PlayerController(this);
        Sprite = (AnimatedSprite) GetNode("Sprite");

        RayPivot = (Area2D) GetNode("RayPivot");

        UI = (Control) GetNode("UI/ControlUI");
        
        Clock = (RichTextLabel) GetNode("UI/ControlUI/Clock");
        DialogueBox = (Dialogue) GetNode("DialogueBox");
        //MessageLabel = (RichTextLabel) GetNode("UI/ControlUI/Message");
        //MessageLabel.BbcodeText = "";
        
        CurrencyLabel = (RichTextLabel) GetNode("UI/ControlUI/Currency");
        Currency += 5;

        Inventory = new Inventory(5);
        
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
        //// Eating items
        //if (Input.IsActionJustPressed("Player_UseItem") && Inventory.HeldSlot < Inventory.Slots.Count &&
        //    Inventory[Inventory.HeldSlot].item is Crop food && food.IsEdible)
        //{
        //    Stamina += food.StaminaIncrease;
        //    Inventory.Remove(food);
        //}
        
        // Dropping Items
        if (Input.IsActionJustPressed("Player_Drop") && Inventory.HeldSlot < Inventory.Slots.Count && CanMove)
        {
            var Item = (ItemEntity) ((PackedScene)GD.Load("res://src/Items/ItemEntity.tscn")).Instance();
            
            Item.CurrentItem = Inventory[Inventory.HeldSlot];
            Item.Position = Position;
            Inventory.Remove(Inventory[Inventory.HeldSlot]);

            GetParent().GetNode("Items").AddChild(Item);
        }

        if (Input.IsActionJustPressed("Player_UseItem") && CanMove)
        {
            // Handles Item interaction
            if (Inventory.HeldSlot < Inventory.Slots.Count)
            {
                switch (Inventory[Inventory.HeldSlot].item)
                {
                    case Tool T:
                        T.Use(this);
                        break;
                    
                    case Crop T:
                        if (T.IsEdible) T.Eat(this);
                        break;
                    
                    case Seed T:
                        if (!(CollidingInteractable is FarmLand farmLand)) break;
                        farmLand.AddSeed(T);
                        Inventory.Remove(Inventory[Inventory.HeldSlot].item);
                        break;
                    
                    case PlaceableItem T:
                        var itemBody = (PlacedItem) ((PackedScene)GD.Load(T.ScenePath)).Instance();
                        itemBody.CurrentItem = T;
                        itemBody.Position = Position;
            
                        Inventory.Remove(T);
                        GetParent().GetNode("Environment/PlacedItems").AddChild(itemBody);
                        break;
                }
            }
        }
        else if (Input.IsActionJustPressed("Player_Action") && CanMove)
        {
            if (CollidingInteractable == null) return;
            switch (CollidingInteractable)
            {
                case InteractableTile T:
                    T.Interact(this);
                    break;
            }
        }

    }

    //TODO: Add Body-Drag Animation
    private void AnimationHandeling()
    {
        if (Input.IsActionPressed("Player_Left") ||
            (Input.IsActionPressed("Player_Left") && Input.IsActionPressed("Player_UP")))
            {
                Sprite.Play("Left-walk");
            }
            else if (Input.IsActionPressed("Player_Right") ||
                     (Input.IsActionPressed("Player_Right") && Input.IsActionPressed("Player_UP")))
            {
                Sprite.Play("Right-walk");
            }
            else if (Input.IsActionPressed("Player_Up"))
            {
                Sprite.Play("Up-walk");
            }
            else if (Input.IsActionPressed("Player_Down"))
            {
                Sprite.Play("Down-walk");
            }
        
        else
        {
            if (Input.IsActionJustReleased("Player_Left") ||
                (Input.IsActionJustReleased("Player_Left") && Input.IsActionJustReleased("Player_UP")))
            {
                Sprite.Play("Left");
            }
            else if (Input.IsActionJustReleased("Player_Right") ||
                     (Input.IsActionJustReleased("Player_Right") && Input.IsActionJustReleased("Player_UP")))
            {
                Sprite.Play("Right");
            }
            else if (Input.IsActionJustReleased("Player_Up"))
            {
                Sprite.Play("Up");
            }
            else if (Input.IsActionJustReleased("Player_Down"))
            {
                Sprite.Play("Down");
            }
        }
    }

    private void InventoryHandling()
    {
        for (int i = 0; i < Inventory.Slots.Count; i++)
        {
            var icon = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Item");
            var amountLabel = (RichTextLabel) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/AmountLabel");
            if (Inventory[i].item == null) continue;
            
            icon.Texture = Inventory[i].item.Icon;
            amountLabel.BbcodeText = Inventory[i].item.MaxStackAmount > 1
                ? Inventory[i].Amount.ToString() 
                : "";
        }

        for (int i = 0; i < Inventory.Slots.Capacity; i++)
        {
            var icon = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Item");
            var selectBox = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Selection");
            var amountLabel = (RichTextLabel) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/AmountLabel");

            selectBox.Visible = (i == Inventory.HeldSlot);

            if (i < Inventory.Slots.Count) continue;
            icon.Texture = null;
            amountLabel.BbcodeText = "";
        }

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

    private void RayCasting()
    {
        foreach (Area2D RayCast in GetTree().GetNodesInGroup("PlayerRays"))
        {
            var collided = false;
            if (RayCast.GetOverlappingAreas().Count <= 0) continue;
            var collidedTile = RayCast.GetOverlappingAreas()[0];

            switch (collidedTile)
            {
                case InteractableTile T:
                    T.OutLine.Visible = true;
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    CollidingInteractable = T;
                    collided = true;
                    break;
                    
                case ItemEntity T:
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    CollidingInteractable = T;
                    collided = true;
                    break;
                    
                case NPC T:
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    collided = true;
                    break;
                    
                case PlacedItem T:
                    T.PlayerColliding = true;
                    T.PlayerBody = this;
                    CollidingInteractable = T;
                    collided = true;
                    break;
            }

            if (collided) break;
            CollidingInteractable = null;
        }
    }

    private void TimeHandling()
    {
        Clock.BbcodeText = TimeNode.Afternoon
            ? $"Day: {TimeNode.Day}, {TimeNode.Hour}:{TimeNode.Minute:00} PM"
            : $"Day: {TimeNode.Day}, {TimeNode.Hour}:{TimeNode.Minute:00} AM";
    }

    private void StatsHandling()
    {
        var statsUI = (Control) UI.GetNode("Stats");
        
        // Stamina
        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
        
        var staminaBar = (TextureProgress) statsUI.GetNode("Stamina");
        staminaBar.Value = Stamina;
        staminaBar.MaxValue = MaxStamina;
    }

    private void PlayerLogic()
    {
        if (CurrencyLabel != null)
            CurrencyLabel.BbcodeText = $"{Currency}G";

        if (!DialogueBox.IsShown && UI.Visible)
        {
            CanMove = true;
        }
    }

    public void MessagePlayer(string Message)
    {
        DialogueBox.PlayerBody = this; 
        if(!DialogueBox.IsShown && CanMove)
            DialogueBox.Announce(Message);
    }
    
}

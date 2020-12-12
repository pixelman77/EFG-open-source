using Godot;
using System;
using System.Collections.Generic;
using EvilFarmingGame.Items;
using EvilFarmingGame.Player;
using EvilFarmingGame.Tiles;

public class Player : KinematicBody2D
{
    private PlayerController Controller;
    public Inventory Inventory;
    private AnimatedSprite Sprite;

    public float Currency = 0;

    public bool CanMove = true;
    
    public Node2D RayPivot;
    public static Vector2 Velocity;

    public Control UI;
    
    private RichTextLabel Clock;
    public DayNight TimeNode;

    private RichTextLabel MessageLabel;
    private Dialogue DialogueBox;
    private RichTextLabel CurrencyLabel;

    public override void _Ready()
    {
        Controller = new PlayerController(this);
        Sprite = (AnimatedSprite) GetNode("Sprite");

        RayPivot = (Area2D) GetNode("RayPivot");

        UI = (Control) GetNode("UI/ControlUI");
        
        Clock = (RichTextLabel) GetNode("UI/ControlUI/Clock");
        DialogueBox = (Dialogue) GetNode("DialogueBox");
        MessageLabel = (RichTextLabel) GetNode("UI/ControlUI/Message");
        MessageLabel.BbcodeText = "";
        
        CurrencyLabel = (RichTextLabel) GetNode("UI/ControlUI/Currency");
        Currency += 5;

        Inventory = new Inventory(5);
        
        if(Owner.HasNode("DayNight"))
            TimeNode = (DayNight) Owner.GetNode("DayNight");
        else
            GD.Print("WARNING: No DayNight Node, day-night system won't be implemented in scene. (Crops won't grow, daylight amount won't change, etc.)");
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
        RayCasting();
        if(TimeNode != null) TimeHandling();
    }

    public override void _Input(InputEvent @event)
    {
        // Dropping Items
        if (Input.IsActionJustPressed("Player_Drop") && Inventory.HeldSlot < Inventory.Items.Count)
        {
            var Item = (ItemEntity) ((PackedScene)GD.Load("res://src/Items/ItemEntity.tscn")).Instance();
            
            Item.CurrentItem = Inventory[Inventory.HeldSlot];
            Item.Position = Position;
            Inventory.Remove(Inventory[Inventory.HeldSlot]);

            GetParent().GetNode("Items").AddChild(Item);
        }
        
        // Placing Placeable Items
        if (Input.IsActionJustPressed("Player_Action") && Inventory.HeldSlot < Inventory.Items.Count &&
            Inventory[Inventory.HeldSlot] is PlaceableItem item)
        {
            var itemBody = (PlacedItem) ((PackedScene)GD.Load(item.ScenePath)).Instance();
            itemBody.CurrentItem = item;
            itemBody.Position = Position;
            
            GetParent().GetNode("Environment/PlacedItems").AddChild(itemBody);
            Inventory.Remove(item);
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
        for (int i = 0; i < Inventory.Items.Count; i++)
        {
            Sprite Icon = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Item");
            Sprite SelectBox = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Selection");

            if (Inventory[i] != null) Icon.Texture = Inventory[i].Icon;
        }

        for (int i = 0; i < Inventory.Items.Capacity; i++)
        {
            Sprite Icon = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Item");
            Sprite SelectBox = (Sprite) GetNode($"UI/ControlUI/Inventory/InventorySlot{i + 1}/Selection");

            if (i == Inventory.HeldSlot)
                SelectBox.Show();
            else
                SelectBox.Hide();

            if (Inventory.Items.Count <= i) Icon.Texture = (Texture) GD.Load("res://src/NoTexture.png");
        }

        if (Input.IsActionJustPressed("ui_right"))
        {
            if (Inventory.HeldSlot < Inventory.Items.Capacity - 1)
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
            bool collided = false;
            if (RayCast.GetOverlappingAreas().Count > 0)
            {
                var collidedTile = RayCast.GetOverlappingAreas()[0];

                switch (collidedTile)
                {
                    case InteractableTile T:
                        var collidedIntTile = T;
                        collidedIntTile.OutLine.Visible = true;
                        collidedIntTile.PlayerColliding = true;
                        collidedIntTile.PlayerBody = this;
                        collided = true;
                        break;
                    
                    case ItemEntity T:
                        var Item = T;
                        Item.PlayerColliding = true;
                        Item.PlayerBody = this;
                        collided = true;
                        break;
                    
                    case NPC T:
                        var npc = T;
                        npc.PlayerColliding = true;
                        npc.PlayerBody = this;
                        collided = true;
                        break;
                    
                    case PlacedItem T:
                        var PItem = T;
                        PItem.PlayerColliding = true;
                        PItem.PlayerBody = this;
                        collided = true;
                        break;
                }

                if (collided) break;

            }
        }
    }

    private void TimeHandling()
    {
        Clock.BbcodeText = TimeNode.Afternoon
            ? $"Day: {TimeNode.Day}, {TimeNode.Hour}:{TimeNode.Minute:00} PM"
            : $"Day: {TimeNode.Day}, {TimeNode.Hour}:{TimeNode.Minute:00} AM";
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

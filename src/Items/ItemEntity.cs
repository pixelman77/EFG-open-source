using Godot;
using System;
using EvilFarmingGame.Items;

public class ItemEntity : Area2D
{
    [Export()] public string ItemID = "Seeds\\TestSeed";

    public Item CurrentItem;
    private Sprite ItemSprite;
    private Sprite Outline;

    public Player PlayerBody;
    public bool PlayerColliding;
    public bool CanBePickedUp = true;
    
    public override void _Ready()
    {
        if (CurrentItem == null)
        {
            try
            {
                CurrentItem = Database<Item>.Get(ItemID);
            }
            catch
            {
                throw new Exception($"Incorrect item-ID set for {nameof(ItemEntity)}");
            }
        }

        ItemSprite = (Sprite) GetNode("Sprite");
        Outline = (Sprite) GetNode("Outline");
        ItemSprite.Texture = CurrentItem.Icon;
    }

    public override void _PhysicsProcess(float delta)
    {
        Outline.Visible = PlayerColliding;
        PlayerColliding = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Player_Action") && PlayerColliding && PlayerBody.Inventory.Items.Count < PlayerBody.Inventory.Items.Capacity && CanBePickedUp)
        {
            PlayerBody.Inventory.Gain(CurrentItem);
            CanBePickedUp = false;
            QueueFree();
        }
    }
    
}

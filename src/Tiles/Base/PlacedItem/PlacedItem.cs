using Godot;
using System;
using EvilFarmingGame.Items;

public class PlacedItem : Area2D
{
    [Export()] private string ItemID;

    public PlaceableItem CurrentItem;
    private Vector2 PlaceableTileSize;

    public Player PlayerBody;
    public bool PlayerColliding;
    
    public override void _Ready()
    {
        if (CurrentItem == null)
        {
            try
            {
                var item = Database<Item>.Get(ItemID);
                try
                {
                    CurrentItem = (PlaceableItem) item;
                }
                catch { throw new Exception("Given item is not a Placeable Item"); }
            }
            catch { throw new Exception("Invalid ItemID"); }
        }
        
    }
    

    public override void _PhysicsProcess(float delta)
    {
        if (PlayerColliding && Input.IsActionJustPressed("Player_Action"))
        {
            Pickup();
        }

        UpdatePosition();
        PlayerColliding = false;
    }

    private void UpdatePosition()
    {
        var rootScene = (Scene) GetTree().Root.GetNode("Scene");
        PlaceableTileSize = rootScene.PlaceableTileSize;
        
        Position = new Vector2(
            GetNearestViablePosition(Position.x, PlaceableTileSize.x), 
            GetNearestViablePosition(Position.y, PlaceableTileSize.y));
        //GD.Print(Position);
    }

    private static float GetNearestViablePosition(float CurrentPos, float TileSize)
    {
        return (float)Math.Round(CurrentPos / TileSize, MidpointRounding.AwayFromZero) * TileSize;
    }

    private void Pickup()
    {
        PlayerBody?.Inventory.Gain(CurrentItem);
        QueueFree();
    }

}

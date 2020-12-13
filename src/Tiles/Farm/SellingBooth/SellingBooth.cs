using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Tiles;

public class SellingBooth : InteractableTile
{
    public Item HeldItem;

    public override void _PhysicsProcess(float delta)
    {
        if(PlayerColliding && Input.IsActionJustPressed("Player_Action") && PlayerBody.Inventory.HeldSlot < PlayerBody.Inventory.Items.Count)
	    {
            HeldItem = PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot];

            if (HeldItem is Crop crop && crop.IsSellable) {
                PlayerBody.Currency += crop.SellingPrice;
                PlayerBody.Inventory.Remove(HeldItem);
            }
        }
        base._PhysicsProcess(delta);
    }

}

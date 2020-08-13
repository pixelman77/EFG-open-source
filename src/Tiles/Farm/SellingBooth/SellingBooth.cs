using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Tiles;

public class SellingBooth : InteractableTile
{
	public Item HeldItem;

	public override void _Ready()
	{
		InitTile();
	}

	public override void _PhysicsProcess(float delta)
	{
		if(PlayerColliding && Input.IsActionJustPressed("Player_Action") && PlayerBody.Inventory.HeldSlot < PlayerBody.Inventory.Items.Count)
		{
			HeldItem = PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot];

            if (HeldItem is Crop crop) {
                PlayerBody.Currency += crop.Price;
                PlayerBody.Inventory.Remove(HeldItem);
            }
        }
		UpdateTile();
	}


}

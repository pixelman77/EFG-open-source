using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Items.Crops;
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
			
			if(HeldItem.Type == Item.Types.Crop)
			{
				PlayerBody.Currency += Crops.GetCrop(HeldItem.ID).Price;
				PlayerBody.Inventory.Remove(HeldItem);
			}
		}
		UpdateTile();
	}


}

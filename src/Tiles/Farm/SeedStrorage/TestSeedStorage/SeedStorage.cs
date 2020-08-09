using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Player;
using EvilFarmingGame.Tiles;

public class SeedStorage : InteractableTile
{
	[Export] private int Seed;

	public override void _Ready()
	{
		InitTile();
	}

	public override void _PhysicsProcess(float delta)
	{
		if (PlayerColliding)
		{
			var CurrentSeed = Seeds.GetSeed(Seed);

			if (Input.IsActionJustPressed("Player_Action"))
			{
				if ((PlayerBody.Currency >= CurrentSeed.Price) && (PlayerBody.Inventory.Items.Count <= PlayerBody.Inventory.Items.Capacity))
				{
					PlayerBody.Currency -= CurrentSeed.Price;
					PlayerBody.Inventory.Gain(CurrentSeed);
				}
			}
		}
		UpdateTile();
	}

	
}

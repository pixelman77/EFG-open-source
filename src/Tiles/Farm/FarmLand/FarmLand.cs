using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Items.Tools;
using EvilFarmingGame.Objects.Farm.Plants;

public class FarmLand : Area2D
{

	public bool PlayerColliding = false;
	
	private AnimatedSprite Sprite;
	
	public Sprite OutLine;
	
	public Player PlayerBody;
	
	private Plant CurrentPlant;
	private Sprite PlantSeedling;
	private Sprite PlantGrown;
	private Timer PlantGrownTimer;

	private Item HeldItem;

	public enum states
	{
		UnCropped = 0,
		Cropped = 1,
		Planted = 2,
		Grown = 3
	}

	public states State;

	public override void _Ready()
	{
		Sprite = (AnimatedSprite) GetNode("Sprite");
		
		PlantSeedling = (Sprite) GetNode("Plant/Seedling");
		PlantGrown = (Sprite) GetNode("Plant/Grown");

		PlantGrownTimer = (Timer) GetNode("Timers/PlantGrowthTimer");

		OutLine = (Sprite) GetNode("OutLine");
		OutLine.Visible = false;
	}

	public override void _PhysicsProcess(float delta)
	{
		switch (State)
		{
			case states.UnCropped:
				Sprite.Play("UnCropped");
				break;
			case states.Cropped:
				Sprite.Play("Cropped");
				break;
			case states.Planted:
				PlantGrown.Hide();
				PlantSeedling.Show();
				break;
			case states.Grown:
				PlantGrown.Show();
				PlantSeedling.Hide();
				break;
		}

		if (CurrentPlant == null)
		{
			PlantSeedling.Texture = (Texture) GD.Load("res://src/NoTexture.png");
			PlantGrown.Texture = (Texture) GD.Load("res://src/NoTexture.png");
		}
		else
		{
			PlantSeedling.Texture = CurrentPlant.SeedlingTexture;
			PlantGrown.Texture = CurrentPlant.GrownTexture;
		}
		OutLine.Visible = false;
		PlayerColliding = false;
	}

	public override void _Input(InputEvent @event)
	{

		if (PlayerColliding)
		{
			if (Input.IsActionJustPressed("Player_Action"))
			{
				if (PlayerBody.Inventory.HeldSlot < PlayerBody.Inventory.Items.Count)
				{
					HeldItem = PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot];

					if (HeldItem == Tools.BasicHoe)
					{
						State = states.Cropped;
					}

					if (HeldItem.Type == Item.Types.Seed)
					{
						if (State == states.Cropped)
						{
							State = states.Planted;
							PlantGrownTimer.Start();
							
							switch (HeldItem.ID)
							{
								case 0:
									CurrentPlant = Plants.TestPlant;
									PlayerBody.Inventory.Remove(PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot]);
									break;
								case 1:
									CurrentPlant = Plants.TestPlant2;
									PlayerBody.Inventory.Remove(PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot]);
									break;
							}
						}
					}
					
				}

				if (PlayerBody.Inventory.Items.Count < PlayerBody.Inventory.Items.Capacity)
				{
					if (State == states.Grown)
					{
						PlayerBody.Inventory.Gain(CurrentPlant.Crop);
						CurrentPlant = null;
						State = states.UnCropped;
					}
				}
			}
		}
	}

	public void OnPlantGrown()
	{
		State = states.Grown;
	}
	
	
}
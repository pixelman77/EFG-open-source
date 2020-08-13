using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Objects.Farm.Plants;
using EvilFarmingGame.Tiles;

public class FarmLand : InteractableTile
{
	private AnimatedSprite Sprite;

	private Plant CurrentPlant;
	private Sprite PlantSeedling;
	private Sprite PlantGrown;
	private Timer PlantGrownTimer;

	private Item HeldItem;

	public enum states
	{
		UnCropped = 0,
		Cropped,
		Planted,
		Watered,
		Grown
		
	}

	public states State;

	public override void _Ready()
	{
		InitTile();
		
		Sprite = (AnimatedSprite) GetNode("Sprite");
		
		PlantSeedling = (Sprite) GetNode("Plant/Seedling");
		PlantGrown = (Sprite) GetNode("Plant/Grown");

		PlantGrownTimer = (Timer) GetNode("Timers/PlantGrowthTimer");
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
				Sprite.Play("UnCropped");
				PlantGrown.Hide();
				PlantSeedling.Show();
				break;
			case states.Watered:
				Sprite.Play("Watered");
				break;
			case states.Grown:
				Sprite.Play("UnCropped");
				PlantGrown.Show();
				PlantSeedling.Hide();
				break;
			default:
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

		UpdateTile();
	}

	public override void _Input(InputEvent @event)
	{

		if (PlayerColliding)
		{
			if (Input.IsActionJustPressed("Player_Action"))
			{
				if (PlayerBody != null && PlayerBody.Inventory.HeldSlot < PlayerBody.Inventory.Items.Count)
				{

					HeldItem = PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot];
					if (HeldItem is Tool tool) 
					{
						if (tool.Type == ToolTypes.Hoe && State == states.UnCropped) 
						{
							State = states.Cropped;
						}
						else if (tool.Type == ToolTypes.WateringCan && State == states.Planted) 
						{
							State = states.Watered;
							PlantGrownTimer.Start();
						}
					}
					else if (HeldItem is Seed seed) 
					{
						if (State == states.Cropped)
						{
							State = states.Planted;
							CurrentPlant = Database<Plant>.Get(seed.PlantID);
						}
					}
				}

				if (PlayerBody != null && PlayerBody.Inventory.Items.Count < PlayerBody.Inventory.Items.Capacity)
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

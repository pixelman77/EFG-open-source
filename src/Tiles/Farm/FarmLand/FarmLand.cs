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
    private Sprite Hole;

    private int PlantedDay;
    private int PlantedHour;

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
    public bool IsWatered;

    public override void _Ready()
    {
        base._Ready();
        
        Sprite = (AnimatedSprite) GetNode("Sprite");
        
        PlantSeedling = (Sprite) GetNode("Plant/Seedling");
        PlantGrown = (Sprite) GetNode("Plant/Grown");
        Hole = (Sprite) GetNode("Hole");

    }

    public override void _PhysicsProcess(float delta)
    {
        switch (State)
        {
            case states.UnCropped:
                Sprite.Play("UnCropped");
                Hole.Visible = false;
                break;
            case states.Cropped:
                Hole.Visible = true;
                break;
            case states.Planted:
                Sprite.Play("UnCropped");
                PlantGrown.Hide();
                PlantSeedling.Show();
                Hole.Visible = false;
                break;
            case states.Grown:
                Sprite.Play("UnCropped");
                PlantGrown.Show();
                PlantSeedling.Hide();
                Hole.Visible = false;
                break;
        }
        if (IsWatered) Sprite.Play("Watered");

        if (CurrentPlant == null)
        {
            PlantSeedling.Texture = null;
            PlantGrown.Texture = null;
        }
        else
        {
            PlantSeedling.Texture = CurrentPlant.SeedlingTexture;
            PlantGrown.Texture = CurrentPlant.GrownTexture;
        }
        
        if (PlayerBody != null && CurrentPlant != null && IsWatered && PlayerBody.TimeNode.Day == PlantedDay+CurrentPlant.GrowthDuration && PlayerBody.TimeNode.Hour == PlantedHour)
        {
            State = states.Grown;
        }

        base._PhysicsProcess(delta);
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
                    if (HeldItem is Tool tool && PlayerBody.Stamina > 0) 
                    {
                        if (tool.Type == ToolTypes.Hoe && State == states.UnCropped) 
                        {
                            State = states.Cropped;
                            tool.Use(PlayerBody);
                        }
                        else if (tool.Type == ToolTypes.WateringCan && !IsWatered)
                        {
                            IsWatered = true;
                            tool.Use(PlayerBody);
                        }
                    }
                    else if (HeldItem is Seed seed) 
                    {
                        if (State == states.Cropped)
                        {
                            CurrentPlant = Database<Plant>.Get(seed.PlantID);
                            PlantedDay = PlayerBody.TimeNode.Day;
                            PlantedHour = PlayerBody.TimeNode.Hour;
                            PlayerBody.Inventory.Remove(seed);
                            GD.Print(PlayerBody.TimeNode.Hour);
                            State = states.Planted;
                        }
                    }
                }

                if (PlayerBody != null && CurrentPlant != null && PlayerBody.Inventory.Items.Count < PlayerBody.Inventory.Items.Capacity)
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
}

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

        if (PlayerBody != null && CurrentPlant != null && IsWatered &&
            PlayerBody.TimeNode.Day == PlantedDay + CurrentPlant.GrowthDuration &&
            PlayerBody.TimeNode.Hour == PlantedHour)
        {
            State = states.Grown;
            IsWatered = false;
        }

        base._PhysicsProcess(delta);
    }

    public void AddSeed(Seed seed)
    {
        if (State != states.Cropped) return;
        
        CurrentPlant = Database<Plant>.Get(seed.PlantID);
        PlantedDay = PlayerBody.TimeNode.Day;
        PlantedHour = PlayerBody.TimeNode.Hour;
        State = states.Planted;
    }

    public bool CollectPlant()
    {
        if (State != states.Grown) return false;
        
        PlayerBody.Inventory.Gain(CurrentPlant.Crop);
        CurrentPlant = null;
        return true;
    }

    public override void Interact(Player PlayerBody)
    {
        CollectPlant();
    }
    
}

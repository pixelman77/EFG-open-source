using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Player;
using EvilFarmingGame.Tiles;

public class SeedStorage : InteractableTile
{
    [Export] public string SeedID;

    public override void _PhysicsProcess(float delta)
    {
        if (PlayerColliding)
        {
            var CurrentSeed = Database<Item>.Get("Seeds\\"+SeedID);

            if (Input.IsActionJustPressed("Player_Action"))
            {
                if ((PlayerBody.Currency >= CurrentSeed.BuyingPrice) && (PlayerBody.Inventory.Slots.Count < PlayerBody.Inventory.Slots.Capacity))
                {
                    PlayerBody.Currency -= CurrentSeed.BuyingPrice;
                    PlayerBody.Inventory.Gain(CurrentSeed);
                }
            }
        }
        base._PhysicsProcess(delta);
    }

    
}

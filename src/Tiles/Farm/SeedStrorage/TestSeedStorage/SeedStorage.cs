using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Player;
using EvilFarmingGame.Tiles;

public class SeedStorage : InteractableTile
{
    [Export] public string SeedID;

    public override void Interact(Player PlayerBody)
    {
        var currentSeed = Database<Item>.Get("Seeds\\"+SeedID);

        if (!Input.IsActionJustPressed("Player_Action") ||
        !(PlayerBody.Currency >= currentSeed.BuyingPrice) ||
        PlayerBody.Inventory.Slots.Count >= PlayerBody.Inventory.Slots.Capacity) return;
        
        PlayerBody.Currency -= currentSeed.BuyingPrice;
        PlayerBody.Inventory.Gain(currentSeed);
    }
}

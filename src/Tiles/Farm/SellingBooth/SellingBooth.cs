using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Tiles;

public class SellingBooth : InteractableTile
{

    public override void Interact(Player PlayerBody)
    {
        if (!(PlayerBody.Inventory.HeldSlot < PlayerBody.Inventory.Slots.Count)) return;
        Item heldItem = PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot];
        if (!(heldItem is Crop crop) || !crop.IsSellable) return;
        
        PlayerBody.Currency += crop.SellingPrice;
        PlayerBody.Inventory.Remove(heldItem);
    }
    
}

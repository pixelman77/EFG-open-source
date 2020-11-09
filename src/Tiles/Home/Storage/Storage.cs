using System;
using EvilFarmingGame.Player;
using Godot;

namespace EvilFarmingGame.Tiles.Home.Storage
{
    public class Storage : InteractableTile
    {
        public Inventory InternalInventory;

        [Export()] int Capacity = 9;
        
        public override void _Ready()
        {
            base._Ready();
            GD.Print(Capacity);
            InternalInventory = new Inventory(Capacity);
        }

        protected void InventoryHandling()
        {
            // Internal-InventoryHandling
            for (int i = 0; i < InternalInventory.Items.Capacity; i++)
            {
                Sprite Icon = (Sprite) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/Item");
                Sprite SelectBox = (Sprite) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/Selection");

                if (InternalInventory.Items.Count <= i) Icon.Texture = (Texture) GD.Load("res://src/NoTexture.png");
                else Icon.Texture = InternalInventory[i]?.Icon;
            }
            
            // Player-InventoryHandling
            for (int i = 0; i < PlayerBody?.Inventory.Items.Capacity; i++)
            {
                Sprite Icon = (Sprite) GetNode($"UI/Control/PlayerSlots/InventorySlot{i + 1}/Item");
                Sprite SelectBox = (Sprite) GetNode($"UI/Control/PlayerSlots/InventorySlot{i + 1}/Selection");

                if (PlayerBody.Inventory.Items.Count <= i) Icon.Texture = (Texture) GD.Load("res://src/NoTexture.png");
                else Icon.Texture = PlayerBody.Inventory[i]?.Icon;
            }
        }
        
        
        public void SlotClicked(InventorySlot slot)
        {
            if (slot.GetParent().Name == "PlayerSlots")
            {
                var slotIndex = Convert.ToInt32(slot.Name.Replace("InventorySlot", ""));
                if (slotIndex <= PlayerBody.Inventory.Items.Count)
                {
                    InternalInventory.Gain(PlayerBody.Inventory[slotIndex -1]);
                    PlayerBody.Inventory.Remove(PlayerBody.Inventory[slotIndex -1]);
                }
            }
            else if (slot.GetParent().Name == "InventorySlots")
            {
                var slotIndex = Convert.ToInt32(slot.Name.Replace("InventorySlot", ""));
                GD.Print(slotIndex);
                if (slotIndex <= InternalInventory.Items.Count)
                {
                    PlayerBody.Inventory.Gain(InternalInventory[slotIndex -1]);
                    InternalInventory.Remove(InternalInventory[slotIndex -1]);
                }
            }
        }
        
    }
}
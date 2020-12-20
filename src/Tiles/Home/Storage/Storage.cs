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
            for (int i = 0; i < InternalInventory.Slots.Capacity; i++)
            {
                var icon = (Sprite) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/Item");
                //var SelectBox = (Sprite) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/Selection");
                var amountLabel = (RichTextLabel) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/AmountLabel");

                if (InternalInventory.Slots.Count <= i) icon.Texture = null;
                else
                {
                    amountLabel.BbcodeText = InternalInventory[i].Amount.ToString("00");
                    icon.Texture = InternalInventory[i].item.Icon;
                }
            }
            
            // Player-InventoryHandling
            for (int i = 0; i < PlayerBody?.Inventory.Slots.Capacity; i++)
            {
                var Icon = (Sprite) GetNode($"UI/Control/PlayerSlots/InventorySlot{i + 1}/Item");
                //Sprite SelectBox = (Sprite) GetNode($"UI/Control/PlayerSlots/InventorySlot{i + 1}/Selection");
                var AmountLabel = (RichTextLabel) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/AmountLabel");

                if (PlayerBody.Inventory.Slots.Count <= i) Icon.Texture = null;
                else
                {
                    AmountLabel.BbcodeText = InternalInventory[i].Amount.ToString("00");
                    Icon.Texture = InternalInventory[i].item.Icon;
                }
            }
        }
        
        
        public void SlotClicked(InventorySlot slot)
        {
            if (slot.GetParent().Name == "PlayerSlots")
            {
                var slotIndex = Convert.ToInt32(slot.Name.Replace("InventorySlot", ""));
                if (slotIndex > PlayerBody.Inventory.Slots.Count) return;
                
                InternalInventory.Gain(PlayerBody.Inventory[slotIndex -1]);
                PlayerBody.Inventory.Remove(PlayerBody.Inventory[slotIndex -1]);
            }
            else if (slot.GetParent().Name == "InventorySlots")
            {
                var slotIndex = Convert.ToInt32(slot.Name.Replace("InventorySlot", ""));
                if (slotIndex > InternalInventory.Slots.Count) return;
                
                PlayerBody.Inventory.Gain(InternalInventory[slotIndex -1]);
                InternalInventory.Remove(InternalInventory[slotIndex -1]);
            }
        }
        
    }
}
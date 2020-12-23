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
                var amountLabel = (RichTextLabel) GetNode($"UI/Control/InventorySlots/InventorySlot{i + 1}/AmountLabel");

                if (i < InternalInventory.Slots.Count)
                {
                    amountLabel.BbcodeText = InternalInventory[i].item.MaxStackAmount > 1
                        ? InternalInventory[i].Amount.ToString("00")
                        : "";
                    icon.Texture = InternalInventory[i].item.Icon;
                    continue;
                }
                amountLabel.BbcodeText = "";
                icon.Texture = null;
            }
            
            // Player-InventoryHandling
            for (int i = 0; i < PlayerBody.Inventory.Slots.Capacity; i++)
            {
                var icon = (Sprite) GetNode($"UI/Control/PlayerSlots/InventorySlot{i + 1}/Item");
                var amountLabel = (RichTextLabel) GetNode($"UI/Control/PlayerSlots/InventorySlot{i + 1}/AmountLabel");

                if (i < PlayerBody.Inventory.Slots.Count)
                {
                    amountLabel.BbcodeText = PlayerBody.Inventory[i].item.MaxStackAmount > 1
                        ? PlayerBody.Inventory[i].Amount.ToString("00")
                        : "";
                    icon.Texture = PlayerBody.Inventory[i].item.Icon;
                    continue;
                }
                amountLabel.BbcodeText = "";
                icon.Texture = null;
            }
        }
        
        
        public void SlotClicked(InventorySlot slot)
        {
            if (slot.GetParent().Name == "PlayerSlots")
            {
                var slotIndex = Convert.ToInt32(slot.Name.Replace("InventorySlot", ""))-1;
                if (slotIndex >= PlayerBody.Inventory.Slots.Count) return;
                
                InternalInventory.Gain(PlayerBody.Inventory[slotIndex]);
                PlayerBody.Inventory.Remove(PlayerBody.Inventory[slotIndex]);
            }
            else if (slot.GetParent().Name == "InventorySlots")
            {
                var slotIndex = Convert.ToInt32(slot.Name.Replace("InventorySlot", ""))-1;
                if (slotIndex >= InternalInventory.Slots.Count) return;
                
                PlayerBody.Inventory.Gain(InternalInventory[slotIndex]);
                InternalInventory.Remove(InternalInventory[slotIndex]);
            }
        }
        
    }
}
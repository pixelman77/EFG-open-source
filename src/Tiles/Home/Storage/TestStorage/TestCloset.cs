using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Tiles.Home.Storage;

public class TestCloset : Storage
{
    private Control UI;
    
    public override void _Ready()
    {
        Capacity = 10;
        InitStorage();
        
        UI = (Control)GetNode("UI/Control");
    }

    public override void _Process(float delta)
    {
        InventoryHandling();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("Player_Action") && PlayerColliding)
        {
            UI.Show();
            
            if(PlayerBody != null) PlayerBody.CanMove = false;
        }

        if (Input.IsActionJustPressed("Window_Exit"))
        {
            UI.Hide();
            if(PlayerBody != null) PlayerBody.CanMove = true;
        }
        
        //TODO: Add UI for inventory
        //foreach (var item in InternalInventory.Items)
        //{
        //    GD.Print(item?.Name);
        //}
        
        UpdateStorage();
    }

    private void InventoryHandling()
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

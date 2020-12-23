using Godot;
using System;
using EvilFarmingGame.Items;
using EvilFarmingGame.Tiles.Home.Storage;

public class TestCloset : Storage
{
    private Control UI;
    
    public override void _Ready()
    {
        base._Ready();
        
        UI = (Control)GetNode("UI/Control");
        UI.Hide();
    }

    public override void _Process(float delta)
    {
        if (PlayerColliding) InventoryHandling();
    }

    public override void _Input(InputEvent @event)
    {
        if (!Input.IsActionJustPressed("Window_Exit") || PlayerBody == null) return;
        
        UI.Hide();
        PlayerBody.UI.Show();
        PlayerBody.CanMove = true;
    }

    public override void Interact(Player PlayerBody)
    {
        UI.Show();

        if (PlayerBody == null) return;
        PlayerBody.UI.Hide();
        PlayerBody.CanMove = false;
    }
}

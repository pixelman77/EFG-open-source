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
        InventoryHandling();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("Player_Action") && PlayerColliding)
        {
            UI.Show();

            if (PlayerBody != null)
            {
                PlayerBody.UI.Hide();
                PlayerBody.CanMove = false;
            }
        }

        if (Input.IsActionJustPressed("Window_Exit"))
        {
            UI.Hide();
            if (PlayerBody != null)
            {
                PlayerBody.UI.Show();
                PlayerBody.CanMove = true;
            }
        }
        base._PhysicsProcess(delta);
    }

}

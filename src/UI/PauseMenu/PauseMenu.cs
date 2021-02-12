using Godot;
using System;

public class PauseMenu : Control
{

    private bool IsPaused = false;
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Window_Exit"))
        ChangeState();
    }

    public void ChangeState()
    {
        IsPaused = !IsPaused;
        GetNode<Button>("CanvasLayer/Resume").Visible = IsPaused;
        GetNode<Button>("CanvasLayer/Quit").Visible = IsPaused;
        GetTree().Paused = IsPaused;
    }

    private void _on_Resume_pressed()
    {
        ChangeState();
    }

    private void _on_Quit_pressed()
    {
        GetTree().Quit();
    }
}


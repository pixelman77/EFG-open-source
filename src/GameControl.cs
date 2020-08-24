using Godot;
using System;

public class GameControl : Node2D
{
    public static bool Debugging = true;
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Window_fullscr")) 
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }

        if (Input.IsActionJustPressed("Window_exit"))
            GetTree().Quit();

        if (Input.IsActionPressed("Game_FastForward") && Debugging)
            Engine.TimeScale = 15;
        else
            Engine.TimeScale = 1;
    }
}

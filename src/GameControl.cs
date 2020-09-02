using Godot;
using System;

public class GameControl : Node2D
{
    public static bool Debugging = true;
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Window_Fullscr")) 
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
        
        //TODO: Add game menu
        //if (Input.IsActionJustPressed("Window_Exit"))
        //GetTree().Quit();

        if (Input.IsActionPressed("Game_FastForward") && Debugging)
            Engine.TimeScale = 15;
        else
            Engine.TimeScale = 1;
    }
}

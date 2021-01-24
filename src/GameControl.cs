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


        if (Input.IsActionPressed("Game_FastForward") && Debugging)
            Engine.TimeScale = 60;
        else
            Engine.TimeScale = 1;

        if (Input.IsActionJustPressed("Debug_DayIncrease"))
        {
            var timeNode = (DayNight) ((Player)GetTree().GetNodesInGroup("Player")[0]).TimeNode;
            timeNode.NextDay();
        }
    }

    public override void _Process(float delta)
    {
        OS.SetWindowTitle($"ThatEvilFarmingGame | FPS: {Engine.GetFramesPerSecond()}");
    }
}

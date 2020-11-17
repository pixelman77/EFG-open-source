using Godot;
using System;

public class DayNight : Node2D
{
    public AnimationPlayer AnimationPlayer;

    public bool Afternoon = false;
    public int Day = 1;
    [Export] public int Hour = 0;
    [Export] public int Minute = 0;
    
    public override void _Ready()
    {
        //AnimationPlayer = (AnimationPlayer) GetNode("AnimationPlayer");
        //AnimationPlayer.Play("Day-Night");
    }

    public override void _Process(float delta)
    {
        if(Afternoon && Hour > 11)
        {
            NextDay();
        }
        else if(Hour > 12)
        {
            Afternoon = true;
            Hour = 1;
        }
    }

    private void NextMinute()
    {
        Minute++;
        if (Minute == 60)
        {
            Minute = 0;
            Hour++;
        }
    }

    public void NextDay()
    {
        Afternoon = false;
        Hour = 0;
        Day++;
    }
}

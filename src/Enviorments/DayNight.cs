using Godot;
using System;

public class DayNight : Node2D
{
    public AnimationPlayer AnimationPlayer;
    
    [Export()] public Gradient SkyGradient;
    private CanvasModulate LightCanvas;
    
    public bool Afternoon = false;
    public int Day = 1;
    public int MinutesInDay = 0;
    [Export()] public int Hour = 0;
    [Export()] public int Minute = 0;
    
    public override void _Ready()
    {
        LightCanvas = GetNode<CanvasModulate>("CanvasModulate");
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
        LightCanvas.Color = SkyGradient.Interpolate(MinutesInDay*(.0001f*7f));
    }

    private void NextMinute()
    {
        Minute++;
        MinutesInDay++;
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
        Minute = 0;
        MinutesInDay = 0;
        Day++;
    }
}

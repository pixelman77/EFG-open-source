using Godot;
using System;
using EvilFarmingGame.Tiles;

public class Bed : InteractableTile
{
    public override void _Ready()
    {
        InitTile();
    }

    public override void _PhysicsProcess(float delta)
    {

        if (Input.IsActionJustPressed("Player_Action") && PlayerColliding)
        {
            if (PlayerBody != null)
            {
                if (PlayerBody.TimeNode.TimeOfDay <= 17 && PlayerBody.TimeNode.TimeOfDay >= 6)
                    Sleep();
                else
                    PlayerBody.MessagePlayer("Its  not  a  good  idea  to  sleep  while  the  police  are  out.");
            }
        }

        if (PlayerBody != null && !PlayerBody.TimeNode.AnimationPlayer.IsPlaying() && PlayerColliding)
            PlayerBody.TimeNode.AnimationPlayer.Play("Day-Night");

        UpdateTile();
    }

    private void Sleep()
    {
        PlayerBody.TimeNode.AnimationPlayer.Stop();
        PlayerBody.TimeNode.AnimationPlayer.Play("Day-Night");
    }
    
}

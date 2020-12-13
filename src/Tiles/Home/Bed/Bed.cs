using Godot;
using System;
using EvilFarmingGame.Tiles;

public class Bed : InteractableTile
{
    public override void _Input(InputEvent @Event)
    {
        if (Input.IsActionJustPressed("Player_Action") && PlayerColliding)
        {
            if (PlayerBody != null)
            {
                if (PlayerBody.TimeNode.Hour <= 17 && PlayerBody.TimeNode.Hour >= 6)
                    Sleep();
                else
                    PlayerBody.MessagePlayer("Its not a good idea to sleep while the police are out.");
            }
        }
    }

    private void Sleep()
    {
        PlayerBody.TimeNode.NextDay();
        PlayerBody.Stamina = PlayerBody.MaxStamina;
    }
    
}

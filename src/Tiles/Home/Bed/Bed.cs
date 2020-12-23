using Godot;
using System;
using EvilFarmingGame.Tiles;

public class Bed : InteractableTile
{
    private void Sleep()
    {
        PlayerBody.TimeNode.NextDay();
        PlayerBody.Stamina = PlayerBody.MaxStamina;
    }

    public override void Interact(Player PlayerBody)
    {
        if (PlayerBody == null) return;
        
        if (PlayerBody.TimeNode.Hour <= 17 && PlayerBody.TimeNode.Hour >= 6) 
            Sleep();
        else 
            PlayerBody.MessagePlayer("Its not a good idea to sleep while the police are out.");
        
    }
}

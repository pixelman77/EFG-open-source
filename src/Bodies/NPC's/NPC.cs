using Godot;
using System;
using EvilFarmingGame.Tiles;

public class NPC : Area2D
{
    [Export()] private string[] DialogueFiles;
    [Export()] public int DialogueAmount;
    [Export()] private bool HasDialogue;
    
    public Player PlayerBody;
    public bool PlayerColliding;
    public Dialogue DialogueBox;

    public override void _Ready()
    {
        DialogueBox = GetNode<Dialogue>("DialogueBox");
    }

    public override void _PhysicsProcess(float delta)
    {
        PlayerColliding = false;
        GD.Print(PlayerColliding);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Player_Action") && PlayerColliding && HasDialogue && !DialogueBox.IsShown && PlayerBody.CanMove)
        {
            DialogueBox.PlayerBody = PlayerBody;
            DialogueBox.DialogueFiles = DialogueFiles;
            DialogueBox.DialogueAmount = DialogueAmount;
            
            DialogueBox.Show();
        }

        if (!DialogueBox.IsShown && PlayerColliding)
        {
            PlayerBody.CanMove = true;
        }
    }
}

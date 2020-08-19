using Godot;
using System;
using System.Text;

public class WifesBody : KinematicBody2D
{
    private bool PlayerColliding = false;

    public override void _PhysicsProcess(float delta)
    {
        Move();
    }

    private void Move()
    {
        if (PlayerColliding && Input.IsActionPressed("Player_Action"))
            MoveAndSlide(Player.Velocity);
    }

    public void OnCollision(PhysicsBody2D Body)
    {
        if (Body is Player)
            PlayerColliding = true;
    }
    
    public void OnExit(PhysicsBody2D Body)
    {
        if (Body is Player)
            PlayerColliding = false;
    }
}

using Godot;
using System;
using System.Text;

public class WifesBody : KinematicBody2D
{
    private bool PlayerColliding = false;
    private Player PlayerBody;

    public override void _PhysicsProcess(float delta)
    {
        Move();
    }

    private void Move()
    {
        if (PlayerBody == null) return;
        {
            if (PlayerColliding && Input.IsActionPressed("Player_Action"))
            {
                PlayerBody.IsDragging = true;
                PlayerBody.DragginBody = this;
                MoveAndSlide(PlayerBody.Velocity);
            }
            else
            {
                PlayerBody.IsDragging = false;
                PlayerBody.DragginBody = null;
            }
        }
    }

    public void OnCollision(PhysicsBody2D Body)
    {
        if (Body is Player player)
        {
            PlayerBody = player;
            PlayerColliding = true;
        }
    }

    public void OnExit(PhysicsBody2D Body)
    {
        if (Body is Player player)
        {
            PlayerBody = player;
            PlayerColliding = false;
        }
    }
}

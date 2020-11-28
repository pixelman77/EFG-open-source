using Godot;
using System;

public class Torch : PlacedItem
{
    private AnimatedSprite animatedSprite;
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        base._Ready();
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        animatedSprite.Play("Lit");
        animationPlayer.Play("Lit");
    }
}

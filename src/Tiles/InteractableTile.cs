using Godot;
using System;
using EvilFarmingGame;

namespace EvilFarmingGame.Tiles
{
    public abstract class InteractableTile : Area2D
    {
        public Sprite OutLine;
        public bool PlayerColliding = false;
        public global::Player PlayerBody;

        public override void _Ready()
        {
            OutLine = (Sprite) GetNode("OutLine");
            OutLine.Visible = false;
        }

        public override void _PhysicsProcess(float delta)
        {
            OutLine.Visible = false;
            PlayerColliding = false;
        }
    }
}
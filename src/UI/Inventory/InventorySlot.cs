using Godot;
using System;

public class InventorySlot : Node2D
{
    [Signal] public delegate void SlotClicked(InventorySlot slot);
    
    private Sprite Outline;

    public bool Selected = false;

    public override void _Ready()
    {
        Outline = (Sprite) GetNode("Selection");
    }

    //public override void _Process(float delta)
    //{
    //    if(Selected && Input.IsActionJustPressed("Player_Click")) 
    //}

    public void Pressed()
    {
        EmitSignal(nameof(SlotClicked), this);
    }

    public void MouseEntered()
    { 
        Outline.Show();
    }

    public void MouseExited()
    {
        Outline.Hide();
    }

}

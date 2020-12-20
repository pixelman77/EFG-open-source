using Godot;
using System;
using EvilFarmingGame.Items;

public class InventoryDetails : RichTextLabel
{
	public Player PlayerBody;
	private bool Debugmode;
	private Item HeldItem;
	public override void _PhysicsProcess(float delta)
	{
		PlayerBody = GetParent().GetParent().GetParent<Player>();
		if (PlayerBody.Inventory.Slots.Count >= PlayerBody.Inventory.HeldSlot + 1)
		{
			HeldItem = PlayerBody.Inventory[PlayerBody.Inventory.HeldSlot];
		}
		else HeldItem = null;

		Debugmode = GameControl.Debugging;
		if (HeldItem != null)
		{
			this.Text = HeldItem.Name;
			if (Debugmode)
			{
				this.Text += "  " + HeldItem.ID;
			}
			if (Input.IsActionPressed("ui_show_des"))
			{
				this.Text += "\n" + HeldItem.Description;
			}
		}
		else
		{
			this.Text = null;
		}
	}


}

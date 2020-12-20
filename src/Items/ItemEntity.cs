using Godot;
using System;
using EvilFarmingGame.Items;

[Tool]
public class ItemEntity : Area2D
{
	[Export()] public string ItemID;

	public Item CurrentItem;
	private Sprite ItemSprite;
	private Sprite Outline;

	public Player PlayerBody;
	public bool PlayerColliding;
	public bool CanBePickedUp = true;
	
	public override void _Ready()
	{
		ItemSprite = (Sprite) GetNode("Sprite");
		Outline = (Sprite) GetNode("OutLine");
	}

	public override void _PhysicsProcess(float delta)
	{
		Outline.Visible = PlayerColliding;
		PlayerColliding = false;

		if (CurrentItem == null)
		{
			try
			{
				CurrentItem = Database<Item>.Get(ItemID);
			}
			catch
			{
				ItemSprite.Texture = null;
				throw new Exception($"Incorrect item-ID set for {nameof(ItemEntity)}");
			}
		}

		ItemSprite.Texture = CurrentItem.Icon;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("Player_Action") && PlayerColliding && PlayerBody.Inventory.CanBeAdded(CurrentItem) && CanBePickedUp)
		{
			PlayerBody.Inventory.Gain(CurrentItem);
			CanBePickedUp = false;
			QueueFree();
		}
	}
	
}

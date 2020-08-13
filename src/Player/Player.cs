using Godot;
using System;
using System.Collections.Generic;
using EvilFarmingGame.Items;
using EvilFarmingGame.Player;
using EvilFarmingGame.Tiles;

public class Player : KinematicBody2D
{
	private PlayerController Controller;
	public Inventory Inventory;
	private AnimatedSprite Sprite;

	public float Currency = 0;
	
	public Node2D RayPivot;

	private RichTextLabel Clock;
	public DayNight TimeNode;

	private RichTextLabel MessageLabel;
	private RichTextLabel CurrencyLabel;

	private ResourcePreloader Loader = new ResourcePreloader();

	public override void _Ready()
	{
		Controller = new PlayerController(this);
		Sprite = (AnimatedSprite) GetNode("Sprite");

		RayPivot = (Node2D) GetNode("RayPivot");
		
		Clock = (RichTextLabel) GetNode("UI/ControlUI/Clock");
		MessageLabel = (RichTextLabel) GetNode("UI/ControlUI/Message");
		MessageLabel.BbcodeText = "";
		
		CurrencyLabel = (RichTextLabel) GetNode("UI/ControlUI/Currency");
		Currency += 5;

		Inventory = new Inventory();
		Inventory.Items.Add(Database<Item>.Get("Tools\\BasicHoe"));
		Inventory.Items.Add(Database<Item>.Get("Tools\\BasicWateringCan"));

		TimeNode = (DayNight) GetNode("DayNight");
	}

	public override void _PhysicsProcess(float delta)
	{
		Controller.InputMovement(delta, RayPivot);
		PlayerLogic();
		
		InventoryHandling();
		AnimationHandeling();
		
		RayCasting();
		TimeHandling();
		
		//GD.Print(Currency);
	}

	private void AnimationHandeling()
	{	
		
		if (Input.IsActionPressed("Player_Left") ||
			(Input.IsActionPressed("Player_Left") && Input.IsActionPressed("Player_UP")))
			{
				Sprite.Play("Left-walk");
			}
			else if (Input.IsActionPressed("Player_Right") ||
					 (Input.IsActionPressed("Player_Right") && Input.IsActionPressed("Player_UP")))
			{
				Sprite.Play("Right-walk");
			}
			else if (Input.IsActionPressed("Player_Up"))
			{
				Sprite.Play("Up-walk");
			}
			else if (Input.IsActionPressed("Player_Down"))
			{
				Sprite.Play("Down-walk");
			}
		
		else
		{
			if (Input.IsActionJustReleased("Player_Left") ||
				(Input.IsActionJustReleased("Player_Left") && Input.IsActionJustReleased("Player_UP")))
			{
				Sprite.Play("Left");
			}
			else if (Input.IsActionJustReleased("Player_Right") ||
					 (Input.IsActionJustReleased("Player_Right") && Input.IsActionJustReleased("Player_UP")))
			{
				Sprite.Play("Right");
			}
			else if (Input.IsActionJustReleased("Player_Up"))
			{
				Sprite.Play("Up");
			}
			else if (Input.IsActionJustReleased("Player_Down"))
			{
				Sprite.Play("Down");
			}
		}
	}

	private void InventoryHandling()
	{
		for (int i = 0; i < Inventory.Items.Count; i++)
		{
			Sprite Icon = (Sprite) GetNode($"UI/Inventory/Inventory Slot{i + 1}/Item");
			Sprite SelectBox = (Sprite) GetNode($"UI/Inventory/Inventory Slot{i + 1}/Selection");

			if (Inventory[i] != null)
				Icon.Texture = Inventory[i].Icon;
		}

		for (int i = 0; i < Inventory.Items.Capacity; i++)
		{
			Sprite Icon = (Sprite) GetNode($"UI/Inventory/Inventory Slot{i + 1}/Item");
			Sprite SelectBox = (Sprite) GetNode($"UI/Inventory/Inventory Slot{i + 1}/Selection");

			if (i == Inventory.HeldSlot)
				SelectBox.Show();
			else
				SelectBox.Hide();

			if (Inventory.Items.Count <= i)
			{
				Icon.Texture = (Texture) GD.Load("res://src/NoTexture.png");
			}
		}

		if (Input.IsActionJustPressed("ui_right"))
		{
			if (Inventory.HeldSlot < Inventory.Items.Capacity - 1)
				Inventory.HeldSlot++;
		}
		else if (Input.IsActionJustPressed("ui_left"))
		{
			if (Inventory.HeldSlot > 0)
				Inventory.HeldSlot--;
		}
	}

	private void RayCasting()
	{
		foreach (RayCast2D RayCast in GetTree().GetNodesInGroup("PlayerRays"))
		{
			var collidedTile = RayCast.GetCollider();

			if (!(collidedTile is InteractableTile)) break;
			{
				var collidedIntTile = (InteractableTile) collidedTile;

				collidedIntTile.OutLine.Visible = true;
				collidedIntTile.PlayerColliding = true;
				collidedIntTile.PlayerBody = this;
				
				break;
			}
		}
	}

	private void TimeHandling()
	{
		if(TimeNode.Afternoon)
			Clock.BbcodeText = $"{TimeNode.TimeOfDay -12} PM";
		else
			Clock.BbcodeText = $"{TimeNode.TimeOfDay} AM";
	}

	private void PlayerLogic()
	{
		if (CurrencyLabel != null)
			CurrencyLabel.BbcodeText = $"{Currency}G";
	}

	public void MessagePlayer(string Message)
	{
		var Timer = (Timer) GetNode("UI/ControlUI/MessageTimer");

		MessageLabel.BbcodeText = Message;
		Timer.Start();
	}

	public void OnMessageTimer()
	{
		MessageLabel.BbcodeText = "";
	}
}

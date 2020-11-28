using Godot;
using System;

public class Scene : Node2D
{
	[Export()] public readonly Vector2 PlaceableTileSize = new Vector2(8,8);
	[Export()] public readonly bool IsCustomisable;
}

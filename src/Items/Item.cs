using EvilFarmingGame.Objects.Farm.Plants;
using Godot;

namespace EvilFarmingGame.Items
{
	public class Item
	{
		public string Name;
		public string Description;
		public string ID;
		public Texture Icon;

		public float Price;

		public Item(string Name, string IconPath, string Description, string ID, float Price = 0) //Constructor with selling-price
		{
			this.Name = Name;
			this.Description = Description;
			this.ID = ID;
			this.Price = Price;

			Icon = (Texture)GD.Load(IconPath);
		}
	}

	public class Crop : Item
	{
		public Crop(string Name, string IconPath, string Description, string ID, float Price = 0) 
			: base(Name, IconPath, Description, ID, Price) { }
	}

	public class Seed : Item
	{
		public string PlantID { get; }

		public Seed(string Name, string IconPath, string Description, string ID, string plantID, float Price = 0) 
			: base(Name, IconPath, Description, ID, Price)
		{
			PlantID = plantID;
		}
	}

	public class Tool : Item
	{
		public ToolTypes Type { get; }

		public Tool(string Name, string IconPath, string Description, string ID, ToolTypes type, float Price = 0) 
			: base(Name, IconPath, Description, ID, Price)
		{
			Type = type; 
		}
	}

	public class PlaceableItem : Item
	{
		public string ScenePath;
		
		public PlaceableItem(string Name, string IconPath, string Description, string ID, string ScenePath, float Price = 0) 
			: base(Name, IconPath, Description, ID, Price)
		{
			this.ScenePath = ScenePath;
		}

	}

	public enum ToolTypes
	{
		Misc = 0,
		Hoe,
		WateringCan
	}
}

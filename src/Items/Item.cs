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

		public bool IsSellable;
		public float BuyingPrice;
		public float SellingPrice;

		public Item(string Name, string IconPath, string Description, string ID, bool IsSellable, float BuyingPrice = 0, float SellingPrice = 0) // Constructor with selling-price
		{
			this.Name = Name;
			this.Description = Description;
			this.ID = ID;
			
			this.IsSellable = IsSellable;
			this.BuyingPrice = BuyingPrice;
			this.SellingPrice = SellingPrice;

			Icon = (Texture) GD.Load(IconPath);
		}
	}

	public class Crop : Item
	{
		public bool IsEdible;
		public float StaminaIncrease;
		
		public Crop(string Name, string IconPath, string Description, string ID, bool IsEdible, bool IsSellable,
			float StaminaIncrease = 0f, float BuyingPrice = 0, float SellingPrice = 0)
			: base(Name, IconPath, Description, ID, IsSellable, BuyingPrice, SellingPrice)
		{
			this.StaminaIncrease = StaminaIncrease;
			this.IsEdible = IsEdible;
		}
	}

	public class Seed : Item
	{
		public string PlantID;

		public Seed(string Name, string IconPath, string Description, string ID, string plantID, bool IsSellable, float BuyingPrice = 0, float SellingPrice = 0) 
			: base(Name, IconPath, Description, ID, IsSellable, BuyingPrice, SellingPrice)
		{
			PlantID = plantID;
		}
	}

	public class Tool : Item
	{
		public ToolTypes Type { get; }
		public float StaminaCost;

		public Tool(string Name, string IconPath, string Description, string ID, ToolTypes type, float StaminaCost, bool IsSellable, float BuyingPrice = 0, float SellingPrice = 0) 
			: base(Name, IconPath, Description, ID, IsSellable, BuyingPrice, SellingPrice)
		{
			Type = type;
			this.StaminaCost = StaminaCost;
		}

		public void Use(global::Player PlayerBody)
		{
			PlayerBody.Stamina -= StaminaCost;
		}
	}

	public class PlaceableItem : Item
	{
		public string ScenePath;
		
		public PlaceableItem(string Name, string IconPath, string Description, string ID, string ScenePath, bool IsSellable, float BuyingPrice = 0, float SellingPrice = 0) 
			: base(Name, IconPath, Description, ID, IsSellable, BuyingPrice, SellingPrice)
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

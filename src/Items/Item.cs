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

		public int MaxStackAmount;
		public bool IsSellable;
		public float BuyingPrice;
		public float SellingPrice;

		public Item(string Name, string IconPath, string Description, string ID, bool IsSellable, int MaxStackAmount = 64, float BuyingPrice = 0, float SellingPrice = 0) // Constructor with selling-price
		{
			this.Name = Name;
			this.Description = Description;
			this.ID = ID;

			this.MaxStackAmount = MaxStackAmount;
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
		
		public Crop(string Name, string IconPath, string Description, string ID, bool IsEdible, bool IsSellable, int MaxStackAmount,
			float StaminaIncrease = 0f, float BuyingPrice = 0, float SellingPrice = 0)
			: base(Name, IconPath, Description, ID, IsSellable, MaxStackAmount , BuyingPrice, SellingPrice)
		{
			this.StaminaIncrease = StaminaIncrease;
			this.IsEdible = IsEdible;
		}

		public void Eat(global::Player PlayerBody)
		{
			PlayerBody.Stamina += StaminaIncrease;
			PlayerBody.Inventory.Remove(this);
		}
	}

	public class Seed : Item
	{
		public string PlantID;

		public Seed(string Name, string IconPath, string Description, string ID, string plantID, bool IsSellable, int MaxStackAmount, float BuyingPrice = 0, float SellingPrice = 0) 
			: base(Name, IconPath, Description, ID, IsSellable, MaxStackAmount, BuyingPrice, SellingPrice)
		{
			PlantID = plantID;
		}
	}

	public class Tool : Item
	{
		public delegate bool UseFunc(global::Player PlayerBody);
		private UseFunc UseFunction;
		
		public readonly ToolTypes Type;
		public float StaminaCost;

		public Tool(string Name, string IconPath, string Description, string ID, ToolTypes type, float StaminaCost, bool IsSellable, UseFunc UseFunction = null, float BuyingPrice = 0, float SellingPrice = 0) 
			: base(Name, IconPath, Description, ID, IsSellable, 0, BuyingPrice, SellingPrice)
		{
			Type = type;
			this.UseFunction = UseFunction;
			this.StaminaCost = StaminaCost;
		}

		public void Use(global::Player PlayerBody)
		{
			if(UseFunction(PlayerBody)) PlayerBody.Stamina -= StaminaCost;
		}
	}

	public class PlaceableItem : Item
	{
		public string ScenePath;
		
		public PlaceableItem(string Name, string IconPath, string Description, string ID, string ScenePath, bool IsSellable, int MaxStackAmount, float BuyingPrice = 0, float SellingPrice = 0) 
			: base(Name, IconPath, Description, ID, IsSellable, MaxStackAmount, BuyingPrice, SellingPrice)
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

using EvilFarmingGame.Objects.Farm.Plants;
using Godot;

namespace EvilFarmingGame.Items
{
    public class Item
    {
        public string Name;
        public string IconPath;
        public string Discription;
        public int ID;
        public Texture Icon;

        public float Price;

        public Item(string Name, string IconPath, string Discription, int ID, float Price = 0) //Constructor with selling-price
        {
            this.Name = Name;
            this.IconPath = IconPath;
            this.Discription = Discription;
            this.ID = ID;
            this.Price = Price;

            this.Icon = (Texture)GD.Load(IconPath);
        }
    }

    public class Crop : Item
    {
        public Crop(string Name, string IconPath, string Discription, int ID, float Price = 0) : base(Name, IconPath, Discription, ID, Price) { }
    }

    public class Seed : Item
    {
        public string PlantID { get; }

        public Seed(string Name, string IconPath, string Discription, int ID, string plantID, float Price = 0) : base(Name, IconPath, Discription, ID, Price)
        {
            PlantID = plantID;
        }
    }

    public class Tool : Item
    {
        public ToolTypes Type { get; }

        public Tool(string Name, string IconPath, string Discription, int ID, ToolTypes type, float Price = 0) : base(Name, IconPath, Discription, ID, Price)
        {
            Type = type; 
        }
    }

    public enum ToolTypes
    {
        Misc = 0,
        Hoe,
        WateringCan
    }
}
using EvilFarmingGame.Items;
using Godot;

namespace EvilFarmingGame.Objects.Farm.Plants
{
    public class Plant
    {
        public string Name;
        public string Discription;
        public int ID;
        public Item Crop;
        public int GrowthDuration;
        
        public Texture SeedlingTexture;
        public Texture GrownTexture;
        
        public Plant(string Name, string Discription, int ID, string SeedlingTexturePath, string GrownTexturePath, Item Crop, int GrowthDurationDays)
        {
            this.Name = Name;
            this.Discription = Discription;
            this.ID = ID;
            this.Crop = Crop;
            this.GrowthDuration = GrowthDurationDays;
            
            this.GrownTexture = (Texture)GD.Load(GrownTexturePath);
            this.SeedlingTexture = (Texture)GD.Load(SeedlingTexturePath);
        }
        
    }
}
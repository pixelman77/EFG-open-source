using System;
using EvilFarmingGame.Objects.Farm.Plants;

namespace EvilFarmingGame.Items.Crops
{
    public struct Crops
    {    
        //Test Crop
        private static Item[] crops =
        {
            new Item("TestCrop", "res://src/Items/Crops/TestCrop/Texture.png", "A simple tomato", 0, Item.Types.Crop, 10),
            new Item("TestCrop2", "res://src/Items/Crops/TestCrop2/Texture.png", "A simple blue tomato", 1, Item.Types.Crop, 20),
        };
        
        public static Item GetCrop(int ID)
        {
            foreach (Item crop in crops)
            {
                if (crop.ID == ID)
                    return crop;
            }

            Console.WriteLine("Cannot find Item by ID");
            return null;
        }
    }
}
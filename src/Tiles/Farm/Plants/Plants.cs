using System;
using EvilFarmingGame.Items.Crops;

namespace EvilFarmingGame.Objects.Farm.Plants
{
    public struct Plants
    {
        private static Plant[] plants =
        {    
            //TestPlant
            new Plant("Test Plant", "A Plant used for testing and debugging purposes", 0,
                "res://src/Tiles/Farm/Plants/TestPlant/Texture1.png",
                "res://src/Tiles/Farm/Plants/TestPlant/Texture2.png", Crops.GetCrop(0)),
            //TestPlant 2
            new Plant("Test Plant2", "A Plant used for testing and debugging purposes", 1,
                "res://src/Tiles/Farm/Plants/TestPlant2/Texture1.png",
                "res://src/Tiles/Farm/Plants/TestPlant2/Texture2.png", Crops.GetCrop(1))
        };
        
        public static Plant GetPlant(int ID)
        {
            foreach (Plant plant in plants)
            {
                if (plant.ID == ID)
                    return plant;
            }

            Console.WriteLine("Cannot find Item by ID");
            return null;
        }
    }
}
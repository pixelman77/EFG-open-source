using System;
using System.Collections.Generic;

namespace EvilFarmingGame.Items
{
    public struct Seeds
    {
        private static Item[] seeds = 
        {
            new Item("Test Seed", "res://src/Items/Seeds/Test Seed/Texture.png", "A Simple seed used for testing and debugging purposes", 0, Item.Types.Seed, 5),
            new Item("Test Seed2", "res://src/Items/Seeds/Test Seed2/Texture.png", "A Simple seed used for testing and debugging purposes", 1, Item.Types.Seed, 10),
        };

        public static Item GetSeed(int ID)
        {
            foreach (Item item in seeds)
            {
                if (item.ID == ID)
                    return item;
            }

            Console.WriteLine("Cannot find Item by ID");
            return null;
        }

}
}
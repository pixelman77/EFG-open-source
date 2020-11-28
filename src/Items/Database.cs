using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilFarmingGame.Objects.Farm.Plants;

namespace EvilFarmingGame.Items
{
    /// <summary>
    /// Database helper.
    /// To store and retrieve stuff, use <see cref="Database{T}"/>instead.
    /// </summary>
    public static class Database
    {
        public static bool Initialized { get; private set; }

        private static Dictionary<string, Item> Items => new Dictionary<string, Item> {
            // Crops.
            { "Crops\\TestCrop", new Crop("TestCrop", "res://src/Items/Crops/TestCrop/Texture.png", "A simple tomato", "Crops\\TestCrop", 10) },
            { "Crops\\TestCrop2", new Crop("TestCrop2", "res://src/Items/Crops/TestCrop2/Texture.png", "A simple blue tomato", "Crops\\TestCrop2", 20) },

            // Seeds.
            { "Seeds\\TestSeed", new Seed("Test Seed", "res://src/Items/Seeds/Test Seed/Texture.png", "A Simple seed used for testing and debugging purposes", "Seeds\\TestSeed", "TestPlant", 5) },
            { "Seeds\\TestSeed2", new Seed("Test Seed2", "res://src/Items/Seeds/Test Seed2/Texture.png", "A Simple seed used for testing and debugging purposes", "Seeds\\TestSeed2", "TestPlant2", 10) },

            // Tools.
            { "Tools\\BasicHoe", new Tool("Basic Hoe", "res://src/Items/Tools/Hoe/Texture.png", "Not the fanciest but gets the job done", "Tools\\BasicHoe", ToolTypes.Hoe) },
            { "Tools\\BasicWateringCan", new Tool("Basic Watering Can", "res://src/Items/Tools/Watering Can/Texture.png", "A basic watering can", "Tools\\BasicWateringCan", ToolTypes.WateringCan) },
            
            // Placeables.
            { "Placeable\\Torches\\WoodenTorch", new PlaceableItem("Wooden Torch", "res://src/Items/Placeables/WoodenTorch/Texture.png", "A simple torch made by a stick and some lighter fluid", "Placeable\\WoodenTorch", "res://src/Items/Placeables/WoodenTorch/WoodenTorch.tscn") }
        };
        
        private static Dictionary<string, Plant> Plants => new Dictionary<string, Plant> {
            { "TestPlant",  new Plant("Test Plant", "A Plant used for testing and debugging purposes", 0,
                "res://src/Tiles/Farm/Plants/TestPlant/Texture1.png",
                "res://src/Tiles/Farm/Plants/TestPlant/Texture2.png", Database<Item>.Get("Crops\\TestCrop"), 1) },
            
            { "TestPlant2", new Plant("Test Plant2", "A Plant used for testing and debugging purposes", 1,
                "res://src/Tiles/Farm/Plants/TestPlant2/Texture1.png",
                "res://src/Tiles/Farm/Plants/TestPlant2/Texture2.png", Database<Item>.Get("Crops\\TestCrop2"), 1) }
        };

        /// <summary>
        /// Initializes the databases.
        /// Does not call if the databases are already initialized.
        /// </summary>
        public static void Initialize()
        {
            if(Initialized)
                return;

            Initialized = true;

            // The database CREATES dictionaries of items, and then registers them.
            // Therefore the order is important.
            Database<Item>.Register(Items);

            Database<Plant>.Register(Plants);
        }
    }

    /// <summary>
    /// Database which stores items with string Keys.
    /// </summary>
    /// <typeparam name="T">Database Type.</typeparam>
    public static class Database<T>
    {
        private static Dictionary<string, T> items = new Dictionary<string, T>();

        /// <summary>
        /// Retrieves an Item of a given ID.
        /// </summary>
        /// <param name="itemID">Item ID.</param>
        /// <returns>Item, if it exists.</returns>
        public static T Get(string itemID)
        {
            if (!Database.Initialized)
                Database.Initialize();
            if (!items.TryGetValue(itemID, out T item))
                throw new KeyNotFoundException($"No {typeof(T)} with ID {itemID} was found within the database.");

            return item;
        }

        /// <summary>
        /// Registers an item into the database. Replaces items of the same ID.
        /// </summary>
        /// <param name="itemID">Item ID.</param>
        /// <param name="item">Item instance.</param>
        public static void Register(string itemID, T item)
        {
            items.Remove(itemID);
            items.Add(itemID, item);
        }

        /// <summary>
        /// Registers a dictionary of items into the database. Replaces existing IDs.
        /// </summary>
        /// <param name="dictionary">Dictionary of items to register.</param>
        public static void Register(Dictionary<string, T> dictionary)
        {
            foreach (var pair in dictionary)
                Register(pair.Key, pair.Value);
        }
    }
}

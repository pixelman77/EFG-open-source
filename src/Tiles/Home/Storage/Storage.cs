
using EvilFarmingGame.Player;

namespace EvilFarmingGame.Tiles.Home.Storage
{
    public class Storage : InteractableTile
    {
        public Inventory InternalInventory;

        public int Capacity = 10;
        
        public void InitStorage()
        {
            InitTile();
            
            InternalInventory = new Inventory(Capacity);
        }

        public void UpdateStorage()
        {
            UpdateTile();
        }
        
    }
}
using UnityEngine;

namespace Collectibles
{
    [CreateAssetMenu(fileName = "New Collectible Item", menuName = "Inventory Collectible Item", order = 0)]
    public abstract class InventoryCollectibleItem : ScriptableObject // Changed this into an abstract class
    {
        public string itemId;
        public Sprite thumbnail;
        public string itemName;
        public string itemDescription;

        // Getter for base Item class + Optional getter for derived Gift class
        // Serves as a placeholder until we get more general classifications
        public abstract InventoryCollectibleItem GetItem();
        public abstract CubeItem GetCube();
        public abstract GiftItem GetGift();
        public abstract SphereItem GetSphere();
        public abstract TableItem GetTable();
    }
}
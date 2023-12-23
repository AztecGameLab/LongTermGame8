using UnityEngine;
using UnityEngine.Serialization;

namespace Collectibles
{
    [CreateAssetMenu(fileName = "New Collectible Item", menuName = "Inventory Collectible Item", order = 0)]
    public class InventoryCollectibleItem : ScriptableObject
    {
        public string itemId;
        public Sprite thumbnail;
        public string itemName;
        public string itemDescription;
        
    }
}
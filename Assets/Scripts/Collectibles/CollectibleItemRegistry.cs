using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    // [CreateAssetMenu(fileName = "New Collectible Item Registry", menuName = "Collectible Item Registry", order = 0)]
    public class CollectibleItemRegistry : ScriptableObject
    {
        public List<InventoryCollectibleItem> items;
    }
}
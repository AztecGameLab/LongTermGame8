using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectibles
{
    public class PlacedCollectibleItem : MonoBehaviour
    {
        public InventoryCollectibleItem collectibleItem;
        
        public InventoryCollectibleItem Collect()
        {
            gameObject.SetActive(false);
            return collectibleItem;
        }

    }
}
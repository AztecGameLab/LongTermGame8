using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Ltg8.Inventory
{
    public class InventoryData
    {
        public List<InventoryItem> Items = new List<InventoryItem>();
    }

    [Serializable]
    public class InventoryItem
    {
        public Vector2 position;
        public AssetReference itemData;
    }

}

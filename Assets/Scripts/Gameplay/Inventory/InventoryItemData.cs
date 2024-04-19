using System;
using UnityEngine;

namespace Ltg8.Inventory
{
    [Serializable]
    public class InventoryItemData
    {
        public Vector2 position;
        public string itemId;
        
        public ItemData Data
        {
            get => Ltg8.ItemRegistry.FindItem(itemId);
            set => itemId = value.guid;
        }
    }
}

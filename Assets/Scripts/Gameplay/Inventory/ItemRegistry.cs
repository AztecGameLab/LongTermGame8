using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ltg8.Inventory
{
    [CreateAssetMenu]
    public class ItemRegistry : ScriptableObject
    {
        [SerializeField]
        private List<ItemData> itemData;

        public ItemData FindItem(string itemId)
        {
            foreach (ItemData item in itemData)
            {
                if (item.guid == itemId)
                    return item;
            }

            throw new Exception($"Failed to find item {itemId}");
        }
    }
}

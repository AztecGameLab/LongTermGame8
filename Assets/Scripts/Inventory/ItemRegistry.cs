using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    
    /*
     * Description
     * The registry (list) of all of the game's items, used for storing and searching for items
     * NOTE: Complete
     */
    
    [CreateAssetMenu]
    public class ItemRegistry : ScriptableObject
    {
        [SerializeField]
        private List<ItemData> itemData; // The list of items 

        // Finds the desired item using its itemID
        public ItemData FindItem(string itemId)
        {
            foreach (ItemData item in itemData)
            {
                if (item.guid == itemId) // If the items' Global Unique Identifiers match
                    return item; // Return the item
            }

            throw new Exception($"Failed to find item {itemId}"); // Only throws if the item is not found
        }
    }
}

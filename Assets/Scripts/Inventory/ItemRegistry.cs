using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    
    /// <summary>
    /// <b>Description</b>: The registry (list) of all the game's items, used for storing and searching for items
    /// <list type="table">
    ///       <item> <description><b><see cref="itemData"/></b></description> </item>
    ///       <item> <description><b><see cref="FindItem"/></b></description> </item>
    /// </list>
    /// </summary>
    [CreateAssetMenu]
    public class ItemRegistry : ScriptableObject
    {
        /// <summary> The list of item datas </summary>
        [SerializeField]
        private List<ItemData> itemData;

        /// <summary> <b>Description</b>: Finds the desired item using its itemID </summary>
        /// <param name="itemId"> The ID of the item to search for </param>
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

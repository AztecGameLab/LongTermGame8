using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    
     // NOTE: Complete
    
    /// <summary>
    ///   <b>Description</b>: The registry (list) of all the game's items, used for storing and searching for items
    /// <list type="table">
    ///       <item>
    ///           <description><b>itemData</b> - The (Serializable) list of all the game's items </description>
    ///       </item>
    ///       <item>
    ///           <description><b>FindItem()</b> - Uses an itemID to locate and return a game item </description>
    ///       </item>
    ///   </list>
    /// </summary>
    [CreateAssetMenu]
    public class ItemRegistry : ScriptableObject
    {
        [SerializeField]
        private List<ItemData> itemData; // The list of items 

        /// <summary>
        ///   <b>Description</b>: Finds the desired item using its itemID
        /// </summary>
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

using System;
using UnityEngine;

namespace Inventory
{
    
    /*
     * Description
     * This class is responsible for managing the individual item stickers that appear when you open up your inventory or
     * when you pick up an item. This includes having an ItemData as a property, as well as storing the location of
     * the item. (As well as itemID for simplicity sake)
     * NOTE: Complete
     */
    
    [Serializable]
    public class InventoryItemData
    {
        public Vector2 position; // The position of the 2D sticker on the Inventory GUI
        public string itemId; // The ID of the item
        
        /* NOTE: This is a property that makes getters and setters much more simple. With this, you can get the item simply
           NOTE: by typing "___.Data", and you can set the item data without having to worry about setting the GUID */
        public ItemData Data 
        {
            get => Ltg8.Ltg8.ItemRegistry.FindItem(itemId); // Returns the ItemData object for the given itemID
            set => itemId = value.guid; // Sets itemID to the to the GUID of the item "set" is called on
        }
    }
}

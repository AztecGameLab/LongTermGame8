using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using UnityEngine;

namespace Inventory
{
    
    /*
     * Description
     * This class is responsible for the items that are present around the map (Catapult Pieces, Camera Pieces, etc)
     */
    
    public class InventoryItemWorldDisplay : MonoBehaviour
    {
        [SerializeField] private Transform uiParent; // The parent of the displayed item  
        [SerializeField] private string defaultItemId; // The ID of the item in question

        private Transform _activeItem; // The transform of the active, physical item 
        private ItemData _activeData; // The data of the active item

        private void Start()
        {
            if (defaultItemId != string.Empty) // If the defaultItemId is specified
                Display(Ltg8.Ltg8.ItemRegistry.FindItem(defaultItemId)); // Displays this item in the world
        }

        // This function displays the specified item in the overworld
        public void Display(ItemData data)
        {
            if (_activeItem != null) // If there is already an active item
            {
                
                // Destroy this original active item and set the active item and data to null
                Destroy(_activeItem.gameObject); 
                _activeItem = null;
                _activeData = null;
            }
            
            if (data != null) // If the data given in the parameters is not null
            {
                // NOTE: Instantiate(original,parent), clones "original" and makes it a child of "parent"
                // Clones the uiView of the item and makes it a child of uiParent
                _activeItem = Instantiate(data.uiView, uiParent).transform;
                _activeData = data; // Makes the data provided in the parameters the active data 
            }
        }

        // This is the function that will run when the player collects the item
        public void Collect()
        {
            if (_activeData == null) return; // If the active item/data is not null
            InventoryUtil.AddItem(_activeData.guid).Forget(); // Adds the item into the player's inventory
            Destroy(_activeItem.gameObject); // Destroy the item
            _activeItem = null; // Set the active item to null
            _activeData = null; // Set the activate data to null
        }
    }
}

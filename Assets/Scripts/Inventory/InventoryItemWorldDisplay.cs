using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Inventory
{
    
    /// <summary>
    /// <b>Description</b>: Responsible for the game items that are present around the map (Catapult Pieces, Camera Pieces, etc)
    /// <list type="number">
    ///       <item> <description><b><see cref="Display"/></b></description> </item>
    ///       <item> <description><b><see cref="Collect"/></b></description> </item>
    /// </list>
    /// </summary>
    public class InventoryItemWorldDisplay : MonoBehaviour
    {
        [SerializeField] private Transform uiParent; // The parent for the item display prefab  
        [SerializeField] private string defaultItemId; // The ID of the default item

        private Transform _activeItem; // The gameObject of the active, physical item 
        private ItemData _activeData; // The data of the active item

        private void Start()
        {
            if (defaultItemId != string.Empty) // If the defaultItemId is specified
                // Finds the desired ItemData in the ItemRegistry and uses it to display the item in the overworld
                Display(Ltg8.Ltg8.ItemRegistry.FindItem(defaultItemId)); 
        }

        /// <summary> <b>Description</b>: Displays the specified item in the overworld. </summary>
        /// <param name="data">The ItemData that determines what item will be displayed</param>
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
                // Sets activeItem to the item's ui prefab and makes it a child of the uiParent object
                _activeItem = Instantiate(data.uiView, uiParent).transform;
                _activeData = data; // Sets activeData to the ItemData used to display the object
                _activeItem.name = data.itemName;
            }
        }

        
        /// <summary> <b>Description</b>: Runs when the player collects the item </summary>
        /// <remarks> <b>Adds</b> the item into the player's inventory before <b>destroying</b> it </remarks>
        public void Collect()
        {
            if (_activeData == null) return; // Don't run if there isn't an active item
            InventoryUtil.AddItem(_activeData.guid).Forget(); // Adds the item into the player's inventory. 
            Destroy(_activeItem.gameObject); // Destroys the active gameObject
            // Nullifies both the activeItem and its data
            _activeItem = null; 
            _activeData = null; 
        }
    }
}

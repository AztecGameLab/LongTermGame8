using UnityEngine;

namespace Inventory
{
    
    /*
     * Description
     * This script is responsible for the creation of the game's item stickers
     * NOTE: Complete
     */
    
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        // The Global Unique Identification for the item
        public string guid;
        // The name of the item
        public string itemName;
        // The UI/Sticker view of the item 
        public GameObject uiView;
    }
}

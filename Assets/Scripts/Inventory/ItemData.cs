using UnityEngine;

namespace Inventory
{
    
    /// <summary>
    /// <b>Description</b>: Stores the data relating to the game's items
    /// <list type="table">
    ///       <item> <description><b>GUID</b> - The Global Unification Identifier for the item.</description> </item>
    ///       <item> <description><b>itemName</b> - The name of the item.</description> </item>
    ///       <item> <description><b>uiView</b> - The GameObject sticker of the item.</description> </item>
    /// </list>
    /// </summary>
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        public string guid; // Global Unique Identifier, used to identify an item (typically in the inspector)
        
        public string itemName; // The name of the item
        
        public GameObject uiView; // The UI Prefab of the Item (The part of the item you see both in-game & in inventory)
        
    }
}

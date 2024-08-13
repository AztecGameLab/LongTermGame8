using UnityEngine;

namespace Inventory
{
     // NOTE: Doc Complete
    
    /// <summary>
    ///   Description: Stores the data relating to the game's items
    /// <list type="table">
    ///       <item>
    ///           <description><b>GUID</b> - The Global Unification Identifier for the item.</description>
    ///       </item>
    /// 
    ///       <item>
    ///           <description><b>itemName</b> - The name of the item.</description>
    ///       </item>
    ///       <item>
    ///           <description><b>uiView</b> - The GameObject sticker of the item.</description>
    ///       </item>
    ///   </list>
    /// </summary>
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        // Global Unification Identifier, used to identify an item (typically in the inspector)
        public string guid;
        
        // The name of the item
        public string itemName;
        
        // The UI Sticker GameObject of the Item (The part of the item you see both in-game & in inventory)
        public GameObject uiView;
        
    }
}

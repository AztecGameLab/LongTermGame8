using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class IncludeItemTarget : ItemTarget
    {
        [SerializeField]
        private List<string> allowedItems;
        
        public override bool CanReceiveItem(ItemData data)
        {
            foreach (string allowedItemId in allowedItems)
                if (data.guid == allowedItemId) return true;

            return false;
        }
    }
}

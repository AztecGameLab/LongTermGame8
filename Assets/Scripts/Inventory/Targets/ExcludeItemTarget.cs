using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Ltg8.Inventory
{

    public class ExcludeItemTarget : ItemTarget
    {
        [SerializeField]
        private List<string> notAllowedItems;
        
        public override bool CanReceiveItem(ItemData data)
        {
            foreach (string notAllowedItemId in notAllowedItems)
                if (data.guid == notAllowedItemId) return false;

            return true;
        }
    }
}

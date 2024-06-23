using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Ltg8.Inventory
{
    public class MultiItemTarget : ItemTarget
    {
        [SerializeField] private List<Target> targets;

        public override bool CanReceiveItem(ItemData data)
        {
            foreach (Target target in targets)
            {
                if (target.itemGuid == data.guid)
                    return true;
            }
            
            return false;
        }

        public override void ReceiveItem(ItemData item)
        {
            base.ReceiveItem(item);

            foreach (Target target in targets)
            {
                if (target.itemGuid == item.guid)
                    target.onReceived.Invoke();
            }
        }

        [Serializable]
        public struct Target
        {
            public string itemGuid;
            public UnityEvent onReceived;
        }
    }
}

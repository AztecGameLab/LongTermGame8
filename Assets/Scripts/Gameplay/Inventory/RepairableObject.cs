using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Ltg8.Inventory
{
    public class RepairableObject : ItemTarget
    {
        public List<RepairStep> repairSteps;
        public UnityEvent onRepairFinish;
        
        public override bool CanReceiveItem(ItemData data)
        {
            foreach (RepairStep repairStep in repairSteps)
            {
                if (repairStep.targetItem == data && !repairStep.IsComplete)
                {
                    // this is an item we want, and its not completed yet
                    return true;
                }
            }
            
            // we dont want this item, or its already completed
            return false;
        }

        public override void ReceiveItem(ItemData item)
        {
            base.ReceiveItem(item);

            // find the step that we just completed, and complete it
            foreach (RepairStep repairStep in repairSteps)
            {
                if (repairStep.targetItem == item)
                {
                    repairStep.IsComplete = true;
                    repairStep.onComplete?.Invoke();
                }
            }
            
            // check all of the steps to see if they are finished
            bool needsRepairs = false;
            
            foreach (RepairStep repairStep in repairSteps)
            {
                if (!repairStep.IsComplete)
                    needsRepairs = true;
            }
            
            if (!needsRepairs)
                onRepairFinish.Invoke();
        }

        // a single item that needs to be used to repair this object
        [Serializable] public class RepairStep
        {
            public UnityEvent onComplete;
            public ItemData targetItem;
            
            [NonSerialized]
            public bool IsComplete;
        }
    }
}

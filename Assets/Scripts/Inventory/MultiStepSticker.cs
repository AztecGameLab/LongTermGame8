using Cysharp.Threading.Tasks;
using Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Ltg8.Inventory
{
    public class MultiStepSticker : MonoBehaviour
    {
        public string[] requiredItemIds;
        public string combinedStickerId;
        public UnityEvent onCombine;

        private void FixedUpdate()
        {
            // todo: bad
            
            // check to see if you found all items
            int collectedParts = 0;
            
            foreach (string itemId in requiredItemIds)
            {
                foreach (InventoryItemData data in Ltg8.Save.Inventory)
                {
                    if (data.itemId == itemId)
                    {
                        collectedParts++;
                        break;
                    }
                }
            }

            // if you have, remove them and add combined sticker
            if (collectedParts >= requiredItemIds.Length)
            {
                foreach (string itemId in requiredItemIds)
                {
                    foreach (InventoryItemData itemData in Ltg8.Save.Inventory)
                    {
                        if (itemData.itemId == itemId)
                        {
                            Ltg8.Save.Inventory.Remove(itemData);
                            break;
                        }
                    }
                }

                InventoryUtil.AddItem(combinedStickerId).Forget();
                onCombine.Invoke();
            }
        }
    }
}

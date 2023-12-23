using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public class PlayerCollectibleItemController : MonoBehaviour
    {
        public Transform playerLookTransform;
        public List<InventoryCollectibleItem> collectedItems;
        public float collectionRange = Mathf.Infinity;

        private bool IsLookingAtCollectible(out PlacedCollectibleItem item)
        {
            if (Physics.Raycast(playerLookTransform.position, playerLookTransform.TransformDirection(Vector3.forward),
                    out RaycastHit hit, collectionRange))
            {
                if (hit.collider.gameObject.TryGetComponent<PlacedCollectibleItem>(
                        out PlacedCollectibleItem placedCollectibleItem))
                {
                    item = placedCollectibleItem;
                    return true;
                }
            }

            item = null;
            return false;
        }

        public void CollectItem()
        {
            if (!IsLookingAtCollectible(out PlacedCollectibleItem item) ||
                collectedItems.Contains(item.collectibleItem)) return;

            InventoryCollectibleItem collectibleItem = item.Collect();
            collectedItems.Add(collectibleItem);
        }
        
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public class PlayerCollectibleItemController : MonoBehaviour
    {
        public Transform playerLookTransform;
        public List<InventoryCollectibleItem> collectedItems;
        
        private bool IsLookingAtCollectible(out PlacedCollectibleItem item)
        {
            if (Physics.Raycast(playerLookTransform.position, playerLookTransform.TransformDirection(Vector3.forward),
                    out RaycastHit hit, Mathf.Infinity))
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
            
            InventoryCollectibleItem collectibleItem = item.collectibleItem;
            Debug.Log(collectibleItem.itemName + ": " + collectibleItem.itemDescription);
            collectedItems.Add(collectibleItem);
            Destroy(item.gameObject);
        }
        
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
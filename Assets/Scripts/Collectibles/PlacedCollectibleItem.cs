using UnityEngine;

namespace Collectibles
{
    public class PlacedCollectibleItem : MonoBehaviour, IPlayerInteractable
    {
        public InventoryCollectibleItem collectibleItem;
        
        public void Interact(PlayerInteractController playerInteractController)
        {
            Debug.Log(Time.time + ": " + gameObject.name);
            // playerInteractController.Inventory.collectedItems.Add(collectibleItem);
            // gameObject.SetActive(false);
        }

    }
}
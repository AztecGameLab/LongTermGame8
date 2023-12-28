using UnityEngine;

namespace Collectibles
{
    public class PlacedCollectibleItem : MonoBehaviour, IPlayerInteractable
    {
        public InventoryCollectibleItem collectibleItem;
        
        public void Interact(PlayerInteractController playerInteractController)
        {
            playerInteractController.Inventory.collectedItems.Add(collectibleItem);
            gameObject.SetActive(false);
        }

    }
}
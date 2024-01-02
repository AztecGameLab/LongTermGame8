using FMODUnity;

namespace Collectibles
{
    public class PlacedCollectibleItem : PlayerInteractable
    {
        public InventoryCollectibleItem collectibleItem;
        public EventReference collectSound;

        public override void Interact(PlayerInteractController playerInteractController)
        {
            playerInteractController.Inventory.collectedItems.Add(collectibleItem);
            RuntimeManager.PlayOneShot(collectSound, transform.position);
            gameObject.SetActive(false);
        }
        
        
        
    }
}
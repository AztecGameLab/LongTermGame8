using UnityEngine;

namespace Collectibles
{
    [RequireComponent(
        typeof(PlayerCollectibleInventory)
    )]
    public class PlayerInteractController : MonoBehaviour
    {
        public float interactRange = 20;
        public PlayerCollectibleInventory Inventory { get; private set; }

        private void Start()
        {
            Inventory = GetComponent<PlayerCollectibleInventory>();
        }

        private bool TryGetNearbyCollectible(out IPlayerInteractable playerInteractable)
        {
            playerInteractable = null;

            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward),
                    out RaycastHit hit, interactRange))
            {
                return hit.collider.gameObject.TryGetComponent<IPlayerInteractable>(out playerInteractable);
            }

            return false;
        }

        /// <summary>
        /// Tries to find an interactable object in the player's range.
        /// 
        /// If one is found, this calls it's <see cref="M:Collectibles.IPlayerInteractable.Interact(Collectibles.PlayerInteractController)"/>
        /// method, otherwise it does nothing.
        /// </summary>
        public void TryInteract()
        {
            if (!TryGetNearbyCollectible(out IPlayerInteractable playerInteractable))
                return;

            playerInteractable.Interact(this);
        }
    }
}
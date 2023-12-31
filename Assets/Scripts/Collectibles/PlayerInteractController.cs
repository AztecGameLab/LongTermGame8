using System.Collections.Generic;
using System.Linq;
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

            Transform playerTransform = gameObject.transform;
            
            Collider[] objects = Physics.OverlapSphere(playerTransform.position, interactRange);

            HashSet<InteractableAngle> interactables = new HashSet<InteractableAngle>();
            
            foreach (Collider interactCollider in objects)
            {
                if (!interactCollider.TryGetComponent(out IPlayerInteractable interactable)) continue;

                Vector3 objectPosition = interactCollider.transform.position;
                Vector3 playerToObjectVector = objectPosition - playerTransform.position;

                // Debug.DrawRay(playerTransform.position, playerTransform.TransformDirection(Vector3.forward), Color.blue, 500);
                // Debug.DrawRay(playerTransform.position, playerToObjectVector, Color.magenta, 500);
                if (Physics.Raycast(playerTransform.position, playerToObjectVector.normalized, out RaycastHit hit,
                        playerToObjectVector.magnitude))
                {
                    if (!hit.collider.TryGetComponent(out IPlayerInteractable checkInteract))
                        continue;
                }
                
                float angle = Vector3.Angle(playerToObjectVector, playerTransform.TransformDirection(Vector3.forward));
                interactables.Add(new InteractableAngle(interactable, angle));
            }

            if (interactables.Count <= 0)
                return false;
            
            IEnumerable<InteractableAngle> orderedAngles = interactables.OrderBy(interactableAngle => interactableAngle.angle);
            playerInteractable = orderedAngles.First().interactable;
            return true;
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

        private struct InteractableAngle
        {
            public readonly IPlayerInteractable interactable;
            public readonly float angle;

            public InteractableAngle(IPlayerInteractable interactable, float angle)
            {
                this.interactable = interactable;
                this.angle = angle;
            }

        }
        
    }
}
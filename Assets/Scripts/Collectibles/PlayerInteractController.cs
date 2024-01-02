using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Collectibles
{
    [RequireComponent(
        typeof(PlayerCollectibleInventory)
    )]
    public class PlayerInteractController : MonoBehaviour
    {
        public float interactRange = 5;
        public Color highlightColor = new(48, 48, 48); 
        
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

            HashSet<InteractableIntention> interactables = new HashSet<InteractableIntention>();
            
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
                
                // organize by both how close it is to the player's aim, and how close the player is to it
                // Lower values are prioritized, so Angle 0 and Distance 0 would be the highest weight
                float intentionWeight = angle + playerToObjectVector.sqrMagnitude * 2; 
                interactables.Add(new InteractableIntention(interactable, intentionWeight));
            }

            if (interactables.Count <= 0)
                return false;
            
            IEnumerable<InteractableIntention> orderedAngles = interactables.OrderBy(interactableAngle => interactableAngle.intentionWeight);
            playerInteractable = orderedAngles.First().interactable;
            return true;
        }

        /// <summary>
        /// Tries to find an interactable object in the player's range.
        /// 
        /// If one is found, this calls it's <see cref="M:Collectibles.IPlayerInteractable.Interact(Collectibles.PlayerInteractController)"/>
        /// method, otherwise it does nothing.
        /// </summary>
        public void TryInteract(InputAction.CallbackContext context)
        {
            // filter out other input phases, so it only fires once per button press
            if (!context.started)
                return;
            
            if (!TryGetNearbyCollectible(out IPlayerInteractable playerInteractable))
                return;
            
            playerInteractable.Interact(this);
        }

        private void Update()
        {
            if (!TryGetNearbyCollectible(out IPlayerInteractable playerInteractable))
                return;

            if (playerInteractable is PlacedCollectibleItem item)
            {
                item.Highlight(highlightColor);
            }
        }

        
        
        private struct InteractableIntention
        {
            public readonly IPlayerInteractable interactable;
            public readonly float intentionWeight;

            public InteractableIntention(IPlayerInteractable interactable, float intentionWeight)
            {
                this.interactable = interactable;
                this.intentionWeight = intentionWeight;
            }

        }
        
    }
}
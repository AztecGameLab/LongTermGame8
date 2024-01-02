using System;
using FMODUnity;
using UnityEngine;

namespace Collectibles
{
    public class PlacedCollectibleItemFlyToPlayer : PlayerInteractable
    {
        public InventoryCollectibleItem collectibleItem;
        public EventReference collectSound;
        public float flyToPlayerTime = 0.05f;
        public float shrinkTime = 1f;
        public float collectionShrinkSize = 0.2f;
        public float maxSpeed = 5f;
        public float maxScaleSpeed = 10f;

        // Variables for making it go to the player
        private Transform _player;
        private bool _collecting;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _scaleVelocity = Vector3.zero;

        public override void Interact(PlayerInteractController playerInteractController)
        {
            playerInteractController.Inventory.collectedItems.Add(collectibleItem);
            RuntimeManager.PlayOneShot(collectSound, transform.position);
            // gameObject.SetActive(false);
            interactionEnabled = false;
            _player = playerInteractController.transform;
            _collecting = true;
        }

        private void Update()
        {
            if (!_collecting)
                return;

            Transform itemTransform = transform;
            itemTransform.position =
                Vector3.SmoothDamp(itemTransform.position, _player.position + Vector3.down * 1, ref _velocity, flyToPlayerTime, maxSpeed);

            itemTransform.localScale = Vector3.SmoothDamp(itemTransform.localScale, Vector3.one * collectionShrinkSize,
                ref _scaleVelocity, flyToPlayerTime, maxScaleSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("MainCamera"))
            {
                Destroy(gameObject);
            }
        }
    }
}
using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectibles
{
    public class PlacedCollectibleItemShrinkInPlace : PlayerInteractable
    {
        public InventoryCollectibleItem collectibleItem;
        public EventReference collectSound;
        public float timeToShrink = 0.35f;
        private float _elapsedShrinkTime;
        private Vector3 _startScale;
        private bool _collecting;

        public override void Interact(PlayerInteractController playerInteractController)
        {
            playerInteractController.Inventory.collectedItems.Add(collectibleItem);
            RuntimeManager.PlayOneShot(collectSound, transform.position);
            // gameObject.SetActive(false);
            interactionEnabled = false;
            _startScale = transform.localScale;
            _collecting = true;
        }

        private void Update()
        {
            if (!_collecting)
                return;

            Transform itemTransform = transform;

            _elapsedShrinkTime = Math.Min(_elapsedShrinkTime + Time.deltaTime, timeToShrink);
            
            float ratio = _elapsedShrinkTime / timeToShrink;

            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, ratio);
            
            if (_elapsedShrinkTime >= timeToShrink)
            {
                Destroy(gameObject);
            }
        }
    }
}
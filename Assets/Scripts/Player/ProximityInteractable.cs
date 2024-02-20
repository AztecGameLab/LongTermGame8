using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ltg8.Player
{
    public class ProximityInteractable : OverworldBehaviour
    {
        public string promptText;
        public UnityEvent<GameObject> onPlayerInteractStart;
        public UnityEvent<GameObject> onPlayerInteractStop;
        public List<Trigger> proximityTriggers;

        private ProximityInteractableController _controller;

        private void OnEnable()
        {
            _controller = FindAnyObjectByType<ProximityInteractableController>();
            
            foreach (Trigger proximityTrigger in proximityTriggers)
            {
                proximityTrigger.onTriggerEnter.AddListener(HandleObjectEnter);
                proximityTrigger.onTriggerExit.AddListener(HandleObjectExit);
            }
        }

        private void OnDisable()
        {
            _controller.interactablesInRange.Remove(this);
            
            foreach (Trigger proximityTrigger in proximityTriggers)
            {
                proximityTrigger.onTriggerEnter.RemoveListener(HandleObjectEnter);
                proximityTrigger.onTriggerExit.RemoveListener(HandleObjectExit);
            }
        }

        private void HandleObjectEnter(GameObject obj)
        {
            if (obj.TryGetComponent(out PlayerController _))
            {
                _controller.interactablesInRange.Add(this);
            }
        }
        
        private void HandleObjectExit(GameObject obj)
        {
            if (_controller.interactablesInRange.Contains(this))
            {
                _controller.interactablesInRange.Remove(this);
            }
            if (_controller.ActiveInteractable == this)
            {
                _controller.ActiveInteractable = _controller.FindNearestInteractable();
            }
        }
    }
}

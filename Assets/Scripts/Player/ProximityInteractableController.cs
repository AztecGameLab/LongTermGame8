using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ltg8.Player
{
    public class ProximityInteractableController : MonoBehaviour
    {
        public GameObject proximityUiObject;
        public TMP_Text proximityUiText;
        public List<ProximityInteractable> interactablesInRange;
        public GameObject playerBody;

        private ProximityInteractable _activeInteractable;
        
        public ProximityInteractable ActiveInteractable
        {
            get => _activeInteractable;
            set
            {
                if (_activeInteractable != null) _activeInteractable.onPlayerInteractStop.Invoke(playerBody);
                _activeInteractable = value;
                if (_activeInteractable != null) _activeInteractable.onPlayerInteractStart.Invoke(playerBody);
            }
        }

        private void Start()
        {
            proximityUiObject.SetActive(false);
        }

        private void Update()
        {
            if (interactablesInRange.Count <= 0)
            {
                proximityUiObject.SetActive(false);
                return;
            }

            ProximityInteractable nearest = FindNearestInteractable();
            
            proximityUiObject.SetActive(nearest != null);
            proximityUiText.SetText(nearest.promptText);
            
            if (nearest != null)
                proximityUiObject.transform.position = Ltg8.MainCamera.WorldToScreenPoint(nearest.transform.position);
            
            if (Ltg8.Controls.PlayerFreeMovement.Interact.WasPressedThisFrame())
                ActiveInteractable = nearest;
            
            if (Ltg8.Controls.PlayerFreeMovement.Interact.WasReleasedThisFrame())
                ActiveInteractable = null;
        }

        public ProximityInteractable FindNearestInteractable()
        {
            if (interactablesInRange.Count <= 0)
                return null;
            
            ProximityInteractable nearest = interactablesInRange[0];
            float nearestDist = (nearest.transform.position - playerBody.transform.position).sqrMagnitude;
                
            foreach (ProximityInteractable interactable in interactablesInRange)
            {
                float curDist = (interactable.transform.position - playerBody.transform.position).sqrMagnitude;
                
                if (curDist < nearestDist)
                {
                    nearestDist = curDist;
                    nearest = interactable;
                }
            }

            return nearest;
        }
    }
}

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using UnityEngine;

namespace Ltg8.Player
{
    public class RopeAttachmentPoint : MonoBehaviour
    {
        public List<RopeStage> stages;
        public ProximityInteractable topClimbInteractable;
        public ProximityInteractable ropeHolderInteractable;
        public Transform topPosition;
        public string ropeId;

        private int _currentStage;
        public int CurrentStage
        {
            get => _currentStage;
            private set
            {
                _currentStage = value;
                // enable the correct models for this stage
                // add listener to bottom interact, remove old one
                
            }
        }
        
        private void Start()
        {
            topClimbInteractable.onPlayerInteractStart.AddListener(HandleTopInteract);
            ropeHolderInteractable.onPlayerInteractStart.AddListener(HandleRopeCollect);
        }

        public void HandleRopeDeploy()
        {
            InventoryUtil.RemoveItem(ropeId);
            CurrentStage++;

            if (CurrentStage >= stages.Count)
            {
                // disable deploy interactable
            }
        }
        
        private void HandleRopeCollect(GameObject _)
        {
            if (CurrentStage >= 0)
            {
                // enable deploy interactable
                InventoryUtil.AddItem(ropeId).Forget();
                CurrentStage--;
            }
        }

        private void HandleTopInteract(GameObject _)
        {
            // enter climbing state at top
        }

        private void HandleBottomInteract(GameObject _)
        {
            // enter climbing state at bottom
        }

        [Serializable] public class RopeStage
        {
            public GameObject ropeModel;
            public Transform bottomPosition;
            public ProximityInteractable bottomClimbInteractable;
        }
    }
}

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using UnityEngine;

namespace Ltg8.Player
{

    public class RopeAttachmentPoint : ItemTarget
    {
        public List<RopeStage> stages;
        public ProximityInteractable topClimbInteractable;
        public ProximityInteractable ropeHolderInteractable;
        public ItemTarget ropeDeployTarget;
        public Transform topClimbTransform;
        public Transform topDismountTransform;
        public string ropeId;
        
        private int _currentStage;

        public Transform BottomClimbTransform => stages[CurrentStage].bottomClimbTransform;
        public Transform BottomDismountTransform => stages[CurrentStage].bottomDismountTransform;
        public Vector3 BottomToTopVector => topClimbTransform.position - BottomClimbTransform.position;
        public float ClimbDistance => Vector3.Distance(BottomClimbTransform.position, topClimbTransform.position);
        
        public int CurrentStage
        {
            get => _currentStage;
            
            private set
            {
                // enable the correct models for this stage
                for (int i = 0; i < stages.Count; i++)
                    stages[i].ropeModel.SetActive(i <= _currentStage);
                
                // add listener to bottom interact, remove old one
                stages[_currentStage].bottomClimbInteractable.onPlayerInteractStart.RemoveListener(HandleBottomInteract);
                _currentStage = value;
                stages[_currentStage].bottomClimbInteractable.onPlayerInteractStart.AddListener(HandleBottomInteract);
            }
        }
        
        private void Start()
        {
            topClimbInteractable.onPlayerInteractStart.AddListener(HandleTopInteract);
            ropeHolderInteractable.onPlayerInteractStart.AddListener(HandleRopeCollect);
        }

        public override bool CanReceiveItem(ItemData data)
        {
            // we can only accept items if its a rope and if we have more stages
            return CurrentStage < stages.Count - 1 && data.guid == ropeId;
        }

        public override void ReceiveItem(ItemData item)
        {
            base.ReceiveItem(item);
            CurrentStage++;
        }

        private void HandleRopeCollect(GameObject _)
        {
            if (CurrentStage >= 0)
            {
                InventoryUtil.AddItem(ropeId).Forget();
                CurrentStage--;
            }
        }

        private void HandleTopInteract(GameObject _)
        {
            // enter climbing state at top, trust player to get out of it
            FindAnyObjectByType<PlayerRopeClimbingLogic>().EnterRope(this, PlayerRopeClimbingLogic.RopeLocation.Top);
        }

        private void HandleBottomInteract(GameObject _)
        {
            // enter climbing state at bottom, trust player to get out of it
            FindAnyObjectByType<PlayerRopeClimbingLogic>().EnterRope(this, PlayerRopeClimbingLogic.RopeLocation.Bottom);
        }

        [Serializable] public class RopeStage
        {
            public GameObject ropeModel;
            public Transform bottomClimbTransform;
            public Transform bottomDismountTransform;
            public ProximityInteractable bottomClimbInteractable;
        }
    }
}

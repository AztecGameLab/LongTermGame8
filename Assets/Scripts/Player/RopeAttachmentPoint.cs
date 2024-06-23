using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Inventory;
using Ltg8.Inventory;
using UnityEngine;

namespace Ltg8.Player
{

    public class RopeAttachmentPoint : ItemTarget
    {
        public int initialStage;
        public List<RopeStage> stages;
        public ProximityInteractable topClimbInteractable;
        public ProximityInteractable ropeHolderInteractable;
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
                if (value < 0)
                {
                    topClimbInteractable.enabled = false;
                    ropeHolderInteractable.enabled = false;
                }
                else
                {
                    topClimbInteractable.enabled = true;
                    ropeHolderInteractable.enabled = true;
                }
                
                // add listener to bottom interact, remove old one
                if (_currentStage >= 0)
                {
                    stages[_currentStage].bottomClimbInteractable.onPlayerInteractStart.RemoveListener(HandleBottomInteract);
                    stages[_currentStage].bottomClimbInteractable.enabled = false;
                }
                
                _currentStage = value;

                if (_currentStage >= 0)
                {
                    stages[_currentStage].bottomClimbInteractable.onPlayerInteractStart.AddListener(HandleBottomInteract);
                    stages[_currentStage].bottomClimbInteractable.enabled = true;
                }
                
                // enable the correct models for this stage
                for (int i = 0; i < stages.Count; i++)
                    stages[i].ropeModel.SetActive(i <= _currentStage);
            }
        }
        
        private void Start()
        {
            // all ropes start disabled by default
            foreach (RopeStage ropeStage in stages)
            {
                ropeStage.bottomClimbInteractable.enabled = false;
            }
            
            topClimbInteractable.onPlayerInteractStart.AddListener(HandleTopInteract);
            ropeHolderInteractable.onPlayerInteractStart.AddListener(HandleRopeCollect);
            CurrentStage = initialStage;
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
    }
}

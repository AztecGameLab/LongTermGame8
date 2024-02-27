using System;
using DefaultNamespace;
using poetools.Core.Abstraction;
using UnityEngine;

namespace Ltg8.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        
        [SerializeField] private PhysicsComponent player;
        [SerializeField] private Animator animatorController;
        private float _playerInitialMagnitude;
        private bool _isIdle;

        private void Start()
        {
            animatorController.SetBool(IsIdle, true);
            // Debug.Log(player.Velocity.magnitude);
            // Initial magnitude of Sigmund for logic checks (temp const due to bugs)
            _playerInitialMagnitude = player.Velocity.magnitude;
        }

        private void Update()
        {
            CheckPlayerIdleState(player.Velocity.magnitude);
        }

        private void CheckPlayerIdleState(float currentMagnitude)
        {
            // TO DO: Pass in layer to ChangeAnimationState
            Debug.Log(currentMagnitude);
            // If Sigmund isn't moving, plays animation
            if (currentMagnitude <= _playerInitialMagnitude)
            {
                Debug.Log("Sigmund isn't moving");
                ChangeAnimationState(true);
            }
            else
            {
                ChangeAnimationState(false);
            }
        }

        public void CreateAnimationList(Animator test)
        {
            
        }

        private void ChangeAnimationState(bool newState)
        {
            animatorController.SetBool(IsIdle, newState);
        }

        private void OnGUI()
        {
            // Debug UI code
            // static cl
            GUILayout.Label("Hello World");
        }
    }
}
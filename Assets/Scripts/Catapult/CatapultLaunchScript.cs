using System;
using System.Collections.Generic;
using Animation;
using Audio;
using Ltg8.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Catapult
{
    public class CatapultLaunchScript : MonoBehaviour
    {
        // GameObject variables representing the projectile at two different stages: Loaded & Mid-Air
        private GameObject _projectile;
        private GameObject _launchedObject;
        
        // GameObject variables representing different elements of the catapult
        [SerializeField] private GameObject catapultSpoon;
        [SerializeField] private GameObject catapultPlatform;
        [SerializeField] private GameObject catapultBasket;
        [SerializeField] private GameObject enterProx; // Proximity Interactable for entering the catapult
        [SerializeField] private GameObject basketProx; // Proximity Interactable for activating the catapult 

        // Developer-entered values to modify the range of the catapult
        [SerializeField] private int distance;
        [SerializeField] private float verticalY; // Y-value of the destination

        // Int-value that keeps track of the catapult's current state (Idle, Preparing, Loading, Launching)
        //private int _catapultState;
        public enum CatapultState
        {
            Idle = 0,
            Preparing = 1,
            Loading = 2,
            Launching = 3
        };

        private CatapultState _catapultState;

        // Variables responsible for the audio-functions 
        [SerializeField] private AudioController audioController; // Script for controlling audio
        [SerializeField] private AudioSource audioSource; 
        [SerializeField] private AudioClip[] audios;
        
        // Variables responsible for animation functions
        [SerializeField] private AnimationController animationController; // Script for controlling animations
        private Animator _catapultAnimator;
        
        [SerializeField] private CogTurning cog; // Script for managing how long cogs turn for during animations

        private void Start()
        {
            // Sets and deactivates the catapult Animator
            _catapultAnimator = catapultPlatform.GetComponent<Animator>();
            _catapultAnimator.enabled = false;
            
            /* Disables the MultiItemTarget script for the basket, primarily to avoid an error that occurs
             when the game loads*/
            catapultBasket.GetComponent<IncludeItemTarget>().enabled = false;
            
            // Ensures that the "Enter" & "Basket Launch" Proximity interactables are initially disabled
            ToggleProximities();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable Unity.PerformanceAnalysi
        public void SetProjectile(GameObject newProjectile)
        {
            _projectile = newProjectile;
            
            // Keeps the projectile conjoined to the catapult basket during the animation sequence
            catapultBasket.GetComponent<FixedJoint>().connectedBody =
                _projectile ? _projectile.GetComponent<Rigidbody>() : null;
        }

        // This method performs separate functions, depending on the current catapult state
        public void PrepareCatapult()
        {
            switch (_catapultState)
            {
                // If the catapult is in the middle of an animation, return
                case CatapultState.Preparing or CatapultState.Launching : 
                    return;
                case CatapultState.Loading: // If the catapult is already prepared, activate launch sequence
                {
                    NextState();
                    if (_projectile)
                    {
                        _launchedObject = _projectile; 
                        ToggleProjectilePhysics(); // Enable projectile's "launching" physics
                        Invoke(nameof(LaunchProjectile), 0.55f);
                    }
                    audioController.SetParameters(audioSource, audios[1]); // Sets the launch audio
                    audioController.PlayAudio(); // Plays the launch audio
                    // Plays the catapult launch animation
                    animationController.PlayandPauseAnimation(_catapultAnimator, 2f, 4.5f);
                    cog.SetTime(2.25f); // Cogs set to rotate for 2 seconds
                    ToggleProximities(); // Disables the "Enter" & "Basket Launch" Proximity Interactables
                    Invoke(nameof(NextState), 2.75f); 
                    break;
                }
                default: // If the catapult is in its idle state, prepare it
                    NextState();
                    audioController.SetParameters(audioSource,audios[0]); // Sets the wind-up audio
                    PlayAndPauseAudio(); // Plays the wind-up audio
                    Invoke(nameof(PlayAndPauseAudio), 5.5f); // Stops the wind-up audio after 5.5 seconds
                    // Plays the catapult wind-up animation
                    animationController.PlayandPauseAnimation(_catapultAnimator, 1f, 5.5f);
                    cog.SetTime(5.5f); // Cogs set to rotate for 5.5 seconds
                    Invoke(nameof(ToggleBasketLoadable),5.5f); // Enable ItemTarget function
                    Invoke(nameof(NextState), 5.75f);
                    // Enable "Enter" & "Basket Launch" Proximity Interactables
                    Invoke(nameof(ToggleProximities), 6.25f); 
                    break;
            }
        }

        private void LaunchProjectile()
        {
            SetProjectile(null); // Removes projectile now that it has been launched
            ProjectileAudio(_launchedObject); 
            ToggleBasketLoadable(); // Disables Basket's ItemTarget functionality
            Invoke(nameof(ToggleProjectilePhysics), 0.5f); // Enables projectile's "launched" physics
            // Sets the catapult's launch angle based on the rotation of the spoon
            var launchAngle = catapultSpoon.transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
            var forward = catapultSpoon.transform.forward;
            // Stores the necessary horizontal velocity 
            var velocityH = (float)(CalculateVelocity(launchAngle) * Math.Cos(launchAngle));
            // Stores the necessary vertical velocity
            var velocityV = (float)(CalculateVelocity(launchAngle) * Math.Sin(launchAngle));
            // Horizontal velocity broken down further into necessary X and Z velocities
            var velocityX = -velocityH * forward.x;
            var velocityZ = -velocityH * forward.z;
            var launchedObjectRb = _launchedObject.GetComponent<Rigidbody>();
            // Sets the gravity and velocity of the launched projectile, allowing its trajectory
            launchedObjectRb.useGravity = true;
            launchedObjectRb.velocity = new Vector3(velocityX, velocityV, velocityZ);
        }

        // This method determines the velocity needed for the projectile to travel the desired distance
        private float CalculateVelocity(float launchAngle)
        {

            // Determines the vertical distance between the projectile & its destination
            var verticalDifference = _launchedObject.transform.position.y - verticalY;
            
            /* The rest of the function is based off a formula for velocity when given the
             * Distance
             * Launch Angle
             * Vertical Distance
             This formula should get you the approximate velocity needed to cover the inputted distance,
             in addition to creating a somewhat realistic trajectory */
            
            var numerator = Math.Sqrt(472.0380705f) * (distance + 1);

            var denominator = Math.Sqrt(96.2361f) * Math.Sqrt(
                Math.Cos(launchAngle) * (
                    (distance + 1) * Math.Sin(launchAngle) +
                    verticalDifference * Math.Cos(launchAngle)
                )
            );
            
            return (float)(numerator / denominator);
        }

        // Changes the catapult-state: 0-Idle, 1-Preparing, 2-Load, 3-Launching
        private void NextState()
        {
            if (_catapultState is CatapultState.Launching) { _catapultState = 0; return; }
            _catapultState++;
        }

        /* Toggles the "Enter" and "Basket Launch" Proximity Interactables. These interactables are what
         allow the player to enter the catapult and launch themselves while inside the basket. */
        private void ToggleProximities()
        {
            switch (_catapultState)
            {
                case CatapultState.Loading: // Catapult is ready to launch
                    enterProx.SetActive(true);
                    break;
                case CatapultState.Idle:
                case CatapultState.Preparing:
                case CatapultState.Launching:
                default: // Catapult is not ready to launch
                    enterProx.SetActive(false); 
                    basketProx.SetActive(false);
                    break;
            }
        }

        /* Toggles the projectile's physics, based on whether the projectile is a character or object,
         and whether it is "launching" or "launched" */
        private void ToggleProjectilePhysics()
        {
            // If the projectile is a character, and is "launching", disable its CharacterController before launching
            if (_launchedObject.TryGetComponent(out CharacterController characterController) && characterController.enabled)
            {
                _launchedObject.GetComponent<CharacterController>().enabled = false;
                return;
            }
            // If the projectile is "launching", disable its BoxCollider and Trigger before launching
            if(_launchedObject.GetComponent<BoxCollider>().enabled)
            {
                _launchedObject.GetComponent<BoxCollider>().enabled = false;
                _launchedObject.transform.Find("Trigger").gameObject.SetActive(false);
                return;
            }
            // If the projectile has been "launched", re-enable its Colliders during flight
            _launchedObject.GetComponent<BoxCollider>().enabled = true;
            _launchedObject.GetComponent<SphereCollider>().enabled = true;
            StartCoroutine(RemoveLaunchedObject());
        }

        /* Toggles the Basket's ItemTarget functionalities, which is what allows items to be loaded
         into the catapult. */
        private void ToggleBasketLoadable()
        {
            // Toggles basket between Loadable (L-6) & Non-Loadable (L-Default) layers
            catapultBasket.layer = catapultBasket.layer == default ? 6 : default;
            var targetScript = catapultBasket.GetComponent<MultiItemTarget>();
            // Toggles the SingleItemTarget Script 
            targetScript.enabled = !targetScript.enabled;
        }
        
        public GameObject GetLaunchedObject()
        {
            return _launchedObject;
        }

        public CatapultState GetCatapultState()
        {
            return _catapultState;
        }

        // Allows an audio to be toggled 
        public void PlayAndPauseAudio()
        {
            if(audioSource.isPlaying){audioController.StopAudio();}
            else{audioController.PlayAudio();}
        }

        // Method for setting the "Thud" audio for when a projectile lands
        private void ProjectileAudio(GameObject projectile)
        {
            var projectileSource = projectile.AddComponent<AudioSource>();
            var projectileController = projectile.AddComponent<AudioController>();
            projectileController.SetParameters(projectileSource, audios[2]); // Sets up the "Thud" audio
            // Eeerrm excuse me, what could this be? Who put this here? 
            if (Random.Range(0, 20) == 9 && _launchedObject)
            {
                projectileController.PlayAudio(projectileSource, audios[3], 0);
            }
        }

        private IEnumerator<WaitUntil> RemoveLaunchedObject()
        {
            yield return new WaitUntil(()=> _launchedObject.TryGetComponent(out SphereCollider _));
            _launchedObject = null;
        }
        
    }
}
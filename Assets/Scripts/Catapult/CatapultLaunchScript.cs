using System;
using Animation;
using Ltg8.Inventory;
using UnityEngine;

namespace Catapult
{
    public class CatapultLaunchScript : MonoBehaviour
    {
        private GameObject _projectile;
        private GameObject _launchedObject;
        
        [SerializeField] private GameObject catapultSpoon;
        [SerializeField] private GameObject catapultPlatform;
        [SerializeField] private GameObject catapultBasket;
        [SerializeField] private GameObject enterProx;
        [SerializeField] private GameObject basketProx;

        [SerializeField] private int distance;
        [SerializeField] private float verticalY;

        private int _catapultState;

        [SerializeField] private AnimationController animationController;
        private Animator _catapultAnimator;
        
        [SerializeField] private CogTurning cog;

        private void Start()
        {
            _catapultAnimator = catapultPlatform.GetComponent<Animator>();
            _catapultAnimator.enabled = false;
            catapultBasket.GetComponent<SingleItemTarget>().enabled = false;
            ToggleProximities();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable Unity.PerformanceAnalysis
        public void SetProjectile(GameObject newProjectile)
        {
            _projectile = newProjectile;
            catapultBasket.GetComponent<FixedJoint>().connectedBody =
                _projectile ? _projectile.GetComponent<Rigidbody>() : null;
        }

        public void PrepareCatapult()
        {
            switch (_catapultState)
            {
                case 1 or 3:
                    return;
                case 2:
                {
                    if (_projectile)
                    {
                        _launchedObject = _projectile;
                        ToggleProjectilePhysics();
                        Invoke(nameof(LaunchProjectile), 0.55f);
                    }
                    animationController.PlayandPauseAnimation(_catapultAnimator, 2f, 4.5f);
                    cog.SetTime(2f);
                    NextState();
                    ToggleProximities();
                    Invoke(nameof(NextState), 2.75f);
                    break;
                }
                default:
                    animationController.PlayandPauseAnimation(_catapultAnimator, 1f, 5.5f);
                    cog.SetTime(5.5f);
                    NextState();
                    Invoke(nameof(ToggleBasketLoadable),5.5f);
                    Invoke(nameof(NextState), 5.75f);
                    Invoke(nameof(ToggleProximities), 6.25f);
                    break;
            }
        }

        private void LaunchProjectile()
        {
            SetProjectile(null);
            ToggleBasketLoadable();
            Invoke(nameof(ToggleProjectilePhysics), 0.5f);
            var launchAngle = catapultSpoon.transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
            var forward = catapultSpoon.transform.forward;
            var velocityH = (float)(CalculateVelocity(launchAngle) * Math.Cos(launchAngle));
            var velocityV = (float)(CalculateVelocity(launchAngle) * Math.Sin(launchAngle));
            var velocityX = -velocityH * forward.x;
            var velocityZ = -velocityH * forward.z;
            var launchedObjectRb = _launchedObject.GetComponent<Rigidbody>();
            launchedObjectRb.useGravity = true;
            launchedObjectRb.velocity = new Vector3(velocityX, velocityV, velocityZ);
        }

        private float CalculateVelocity(float launchAngle)
        {

            var verticalDifference = _launchedObject.transform.position.y - verticalY;
            
            var numerator = Math.Sqrt(472.0380705f) * (distance + 1);

            var denominator = Math.Sqrt(96.2361f) * Math.Sqrt(
                Math.Cos(launchAngle) * (
                    (distance + 1) * Math.Sin(launchAngle) +
                    verticalDifference * Math.Cos(launchAngle)
                )
            );
            
            return (float)(numerator / denominator);
        }

        private void NextState()
        {
            if (_catapultState is 3) { _catapultState = 0; return; }
            _catapultState++;
        }

        private void ToggleProximities()
        {
            switch (_catapultState)
            {
                case 2:
                    enterProx.SetActive(true);
                    break;
                default:
                    enterProx.SetActive(false); 
                    basketProx.SetActive(false);
                    break;
            }
        }

        private void ToggleProjectilePhysics()
        {
            if (_launchedObject.TryGetComponent(out CharacterController characterController) && characterController.enabled)
            {
                _launchedObject.GetComponent<CharacterController>().enabled = false;
                return;
            }
            if(_launchedObject.GetComponent<BoxCollider>().enabled)
            {
                _launchedObject.GetComponent<BoxCollider>().enabled = false;
                return;
            }
            _launchedObject.GetComponent<BoxCollider>().enabled = true;
            _launchedObject.GetComponent<SphereCollider>().enabled = true;
        }

        private void ToggleBasketLoadable()
        {
            catapultBasket.layer = catapultBasket.layer == default ? 6 : default;
            var targetScript = catapultBasket.GetComponent<SingleItemTarget>();
            targetScript.enabled = !targetScript.enabled;
        }
        
        public GameObject GetLaunchedObject()
        {
            return _launchedObject;
        }

        public int GetCatapultState()
        {
            return _catapultState;
        }
    }
}
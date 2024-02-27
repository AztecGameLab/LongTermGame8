using System;
using System.Collections.Generic;
using UnityEngine;

namespace Catapult
{
    public class CatapultLaunchScript : MonoBehaviour
    {

        [SerializeField] private GameObject projectile;
        [SerializeField] private Rigidbody projectile_rb;
        private GameObject _catapult;
        private GameObject catapult_spoon;
        private GameObject catapult_platform;
        private GameObject catapult_basket;

        [SerializeField] private int distance;
        [SerializeField] private float land_vertical_y;
        private float launch_angle;

        private float vertical_difference;
        private bool child_found;
        private bool catapult_ready;
        private bool catapult_in_use;

        private Animator catapult_animator;

        [SerializeField] private GameObject cogTurner;
        private CogTurning cog;
        
            void Start()
        {
            _catapult = transform.gameObject;
            catapult_spoon = FindFirstChild(_catapult, "Spoon").gameObject;
            child_found = false;
            catapult_platform = FindFirstChild(_catapult, "Platform").gameObject;
            child_found = false;
            catapult_basket = FindFirstChild(catapult_spoon, "Basket").gameObject;
            catapult_animator = catapult_platform.GetComponent<Animator>();
            vertical_difference = projectile_rb.transform.position.y - land_vertical_y;
            catapult_animator.enabled = false;
            cog = cogTurner.GetComponent<CogTurning>();
        }

        private void SetProjectile(GameObject projectile)
        {
            this.projectile = projectile;
            projectile_rb = projectile.GetComponent<Rigidbody>();
        }

        public void PrepareCatapult()
        {
            if (catapult_in_use)
            {
                return;
            }

            if (!catapult_ready)
            {
                StartCoroutine(PlayandPauseAnimation(catapult_animator, 5.5f, 1f));
                catapult_ready = !catapult_ready;
                StartCoroutine(Cooldown(catapult_in_use, 6f));
                cog.setTime(5.5f);
                cog.turnCog("Launch");
            }

            else
            {
                StartCoroutine(PlayandPauseAnimation(catapult_animator, 4.5f, 2f));
                catapult_ready = !catapult_ready;
                StartCoroutine(Cooldown(catapult_in_use, 2.75f));
                StartCoroutine(DelayAction(LaunchProjectile, 0.55f));
                cog.setTime(2f);
                cog.turnCog("Launch");
            }
        }

        private void LaunchProjectile()
        {
            catapult_basket.GetComponent<FixedJoint>().connectedBody = null;
            launch_angle = catapult_spoon.transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
            float velocity_h = (float)(CalculateVelocity() * Math.Cos(launch_angle));
            float velocity_v = (float)(CalculateVelocity() * Math.Sin(launch_angle));
            float velocity_x = -velocity_h * catapult_spoon.transform.forward.x;
            float velocity_z = -velocity_h * catapult_spoon.transform.forward.z;
            projectile_rb.useGravity = true;
            projectile_rb.velocity = new Vector3(velocity_x, velocity_v, velocity_z);
        }

        private float CalculateVelocity()
        {

            double numerator = Math.Sqrt(472.0380705f) * (distance + 1);

            double denominator = Math.Sqrt(96.2361f) * Math.Sqrt(
                Math.Cos(launch_angle) * (
                    (distance + 1) * Math.Sin(launch_angle) +
                    vertical_difference * Math.Cos(launch_angle)
                )
            );
            return (float)(numerator / denominator);
        }

        private Transform FindFirstChild(GameObject parent, string child_name)
        {
            if (child_found)
            {
                return null;
            }

            Transform child = parent.transform.Find(child_name);
            if (child)
            {
                child_found = true;
                return child;
            }

            if (parent.transform.childCount > 0)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    child = FindFirstChild(parent.transform.GetChild(i).gameObject, child_name);
                    if (child)
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        private IEnumerator<WaitForSeconds> PlayandPauseAnimation(Animator animator, float time, float speed)
        {
            time = time / speed;
            animator.speed = speed;
            animator.enabled = true;
            yield return new WaitForSeconds(time);
            animator.enabled = false;
        }

        private IEnumerator<WaitForSeconds> Cooldown(bool cooldown_value, float time)
        {
            cooldown_value = !cooldown_value;
            yield return new WaitForSeconds(time);
            cooldown_value = !cooldown_value;
        }

        private IEnumerator<WaitForSeconds> DelayAction(Action method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }

        void Update()
        {
            if (!catapult_in_use)
            {
                return;
            }

            if (!catapult_ready)
            {
                catapult_ready = !catapult_ready;
                catapult_in_use = !catapult_in_use;
            }
            else
            {
                if (catapult_spoon.transform.rotation.eulerAngles.x > 15)
                {
                    catapult_spoon.transform.Rotate(-5, 0, 0);
                    if (catapult_spoon.transform.rotation.eulerAngles.x < 30)
                    {
                        launch_angle = catapult_spoon.transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
                        LaunchProjectile();
                    }
                }
                else
                {
                    catapult_ready = !catapult_ready;
                    catapult_in_use = !catapult_in_use;
                }
            }
        }
    }
}
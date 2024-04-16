using UnityEngine;

namespace Catapult
{
    public class CogTurning : MonoBehaviour
    {

        // The gears/cogs that will rotate during animations
        [SerializeField] private GameObject cogLaunch;
        [SerializeField] private GameObject cogRotate;
        [SerializeField] private GameObject cogLever;

        // The launch script inside the catapult
        [SerializeField] private CatapultLaunchScript launch;
    
        // Determines whether to rotate the cogs or not
        private bool _turnRotateCog;

        // Amount of frames the cogs should spin for 
        private float _frames; 

        // Converts seconds to frames
        public void SetTime(float time)
        {
            _frames = (time / Time.deltaTime); 
        }
    
        // Toggles Cog Rotation
        public void RotateCog()
        {
            _turnRotateCog = !_turnRotateCog;
        }

        // Rotates the given object around the Y-Axis for a given angle
        private static void YRotations(GameObject cog, float angle)
        {
            cog.transform.Rotate(0, angle, 0);
        }
    
        void Update()
        {
            // If the Catapult is Preparing
            if (launch.GetCatapultState() == CatapultLaunchScript.CatapultState.Preparing)
            {
                // Rotate until the x-rotation equals 60 (the angle, such _frames, dictates speed)
                if (!(cogLever.transform.eulerAngles.x < 60)) return;
                YRotations(cogLever, 60/_frames);
                YRotations(cogLaunch, 1f);
            }

            //If the Catapult is Launching
            if (launch.GetCatapultState() == CatapultLaunchScript.CatapultState.Launching)
            {
                if (!(cogLever.transform.eulerAngles.x > 1)) return;
                YRotations(cogLever, -60/_frames);
                YRotations(cogLaunch, -5f);
            }
            
            // If the Catapult is Rotating
            if(_turnRotateCog)
            {
                YRotations(cogRotate, 1);
            }
        }
    }
}
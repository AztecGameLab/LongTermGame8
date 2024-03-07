using UnityEngine;

namespace Catapult
{
    public class CogTurning : MonoBehaviour
    {

        [SerializeField] private GameObject cogLaunch;
        [SerializeField] private GameObject cogRotate;
        [SerializeField] private GameObject cogLever;

        [SerializeField] private CatapultLaunchScript launch;
    
        private bool _turnRotateCog;

        private float _frames; 

        public void SetTime(float time)
        {
            _frames = (time / Time.deltaTime);
        }
    
        public void RotateCog()
        {
            _turnRotateCog = !_turnRotateCog;
        }

        private static void YRotations(GameObject cog, float angle)
        {
            cog.transform.Rotate(0, angle, 0);
        }
    
        void Update()
        {
            if (launch.GetCatapultState() == 1)
            {
                if (!(cogLever.transform.eulerAngles.x < 60)) return;
                YRotations(cogLever, 60/_frames);
                YRotations(cogLaunch, 1f);
            }

            if (launch.GetCatapultState() == 3)
            {
                if (!(cogLever.transform.eulerAngles.x > 1)) return;
                YRotations(cogLever, -60/_frames);
                YRotations(cogLaunch, -5f);
            }
            if(_turnRotateCog)
            {
                YRotations(cogRotate, 1);
            }
        }
    }
}
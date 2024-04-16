using UnityEngine;

namespace Misc
{
    public class SyncFov : MonoBehaviour
    {
        public Camera target;
        public Camera self;

        private void Update()
        {
            self.fieldOfView = target.fieldOfView;
        }
    }
}

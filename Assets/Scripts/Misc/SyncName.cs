using UnityEngine;

namespace Misc
{
    [ExecuteAlways]
    public class SyncName : MonoBehaviour
    {
        public GameObject target;

        private void Update()
        {
            if (name != target.name)
                name = target.name;
        }
    }
}

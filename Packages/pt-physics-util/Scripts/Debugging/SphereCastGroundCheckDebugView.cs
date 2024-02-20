using DefaultNamespace;
using UnityEngine;

namespace Debugging
{
    [RequireComponent(typeof(SphereCastGroundCheck))]
    public class SphereCastGroundCheckDebugView : MonoBehaviour
    {
        private SphereCastGroundCheck _groundCheck;

        private void Start()
        {
            _groundCheck = GetComponent<SphereCastGroundCheck>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(_groundCheck.transform.position + _groundCheck.center, _groundCheck.radius);
        }
    }
}

using UnityEngine;

namespace pt_player_3d.Scripts
{
    [RequireComponent(typeof(GroundCheck3d))]
    public class GroundCheck3dDebugView : MonoBehaviour
    {
        private GroundCheck3d _groundCheck3d;
        private float _maxHeight;
        private float _baseY;

        private void Start()
        {
            _groundCheck3d = GetComponent<GroundCheck3d>();
        }

        private void OnGUI()
        {
            if (_groundCheck3d.TryGetGround(out GroundData3d groundData))
            {
                GUILayout.Label($"{name} is grounded.");
                GUILayout.Label($"Normal: {groundData.Normal}");
                GUILayout.Label($"Point: {groundData.Point}");
                GUILayout.Label($"Surface: {groundData.Surface.name}");
                _maxHeight = 0;
                _baseY = transform.position.y;
            }
            else
            {
                GUILayout.Label($"{name} is not grounded.");
            }

            _maxHeight = Mathf.Max(_maxHeight, transform.position.y - _baseY);
            GUILayout.Label($"max height: {_maxHeight}");
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && _groundCheck3d.TryGetGround(out GroundData3d groundData))
            {
                Gizmos.DrawSphere(groundData.Point, 0.1f);
                Gizmos.DrawRay(groundData.Point, groundData.Normal * 2);
            }
        }
    }
}

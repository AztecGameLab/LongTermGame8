using UnityEngine;
namespace DefaultNamespace
{

    public class BoundMultiplier : MonoBehaviour
    {
        public float amount;
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            Bounds bounds = _renderer.bounds;
            bounds.Expand(amount);
            _renderer.bounds = bounds;
        }

        private void OnDrawGizmos()
        {
            _renderer = GetComponent<Renderer>();
            Bounds bounds = _renderer.bounds;
            bounds.Expand(amount);
            _renderer.bounds = bounds;
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
    }
}

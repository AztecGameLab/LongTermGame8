using UnityEngine;

namespace Ltg8.Misc
{
    public class BoundMultiplier : MonoBehaviour
    {
        public float amount;

        private void Awake()
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                Bounds bounds = r.bounds;
                bounds.Expand(amount);
                r.bounds = bounds;
            }

            foreach (Terrain t in GetComponentsInChildren<Terrain>())
                t.patchBoundsMultiplier = new Vector3(amount, amount, amount);
        }
    }
}
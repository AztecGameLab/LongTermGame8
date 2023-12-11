using UnityEngine;
namespace DefaultNamespace
{
    public class TerrainBoundMultiplier : MonoBehaviour
    {
        public float amount;
        private Terrain _terrain;

        private void Awake()
        {
            _terrain = GetComponent<Terrain>();
            _terrain.patchBoundsMultiplier = new Vector3(amount, amount, amount);
        }
    }
}

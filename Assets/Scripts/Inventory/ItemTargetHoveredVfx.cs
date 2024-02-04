using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class ItemTargetHoveredVfx : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        
        public async UniTask Appear(ItemTarget target)
        {
            ParticleSystem.ShapeModule shape = particles.shape;
            shape.meshRenderer = target.GetComponentInChildren<MeshRenderer>();
            await UniTask.Yield();
        }

        public async UniTask Disappear()
        {
            particles.Stop();
            await UniTask.Delay(TimeSpan.FromSeconds(particles.main.duration));
            if (gameObject != null) Destroy(gameObject);
        }
    }
}

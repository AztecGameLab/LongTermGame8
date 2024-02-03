using Cysharp.Threading.Tasks;
using Ltg8.Misc;
using SpaceMystery;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class InventoryItemUiView : MonoBehaviour
    {
        [SerializeField] private float scaleInTime = 1;
        [SerializeField] private float scaleOutTime = 1;
        [SerializeField] private EasingFunctionReference easeAppearFunction;
        [SerializeField] private EasingFunctionReference easeDisappearFunction;
        
        public async UniTask Initialize(InventoryItem item)
        {
            Transform t = transform;
            Vector3 originalScale = t.localScale;
            t.localScale = Vector3.zero; /* start out invisible, with a scale of 0 */
            t.position = item.position; /* load the objects saved position */
            await transform.TweenLocalScale(originalScale, scaleInTime, easeAppearFunction.Resolve()); /* play an animation to become visible */
        }

        public async UniTask Disappear()
        {
            await transform.TweenLocalScale(Vector3.zero, scaleOutTime, easeDisappearFunction.Resolve()); /* play an animation to become invisible */ 
            Destroy(gameObject);
        }
    }
}

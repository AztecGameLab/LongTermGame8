using Cysharp.Threading.Tasks;
using SpaceMystery;
using UnityEngine;

namespace Ltg8.Misc
{
    public delegate T LerpFunction<T>(T from, T to, float t);
    public delegate T Getter<out T>();
    public delegate void Setter<in T>(T value);
    
    public static class UniTaskTween
    {
        public static EasingFunction DefaultEasing { get; set; } = Easing.SmoothStart2;
        
        public static async UniTask TweenLocalScale(this Transform transform, Vector3 target, float duration = 1, EasingFunction easingFunction = null)
        {
            easingFunction ??= DefaultEasing;
            float elapsed = 0;
            Vector3 start = transform.localScale;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = easingFunction(Mathf.Clamp01(elapsed / duration));
                transform.localScale = Vector3.Lerp(start, target, t);
                await UniTask.Yield();
            }

            transform.localScale = target;
        }
    }
}

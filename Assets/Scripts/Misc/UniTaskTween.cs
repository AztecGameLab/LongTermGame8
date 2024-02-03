using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceMystery;
using UnityEngine;

namespace Ltg8.Misc
{
    [Serializable]
    public class TweenSettings /* todo: property drawer niceness */
    {
        public float duration;
        public EasingFunctionReference easingFunction;
    }
    
    public static class UniTaskTween
    {
        public static EasingFunction DefaultEasing { get; set; } = Easing.SmoothStart2;

        public static async UniTask TweenLocalScale(this Transform transform, Vector3 target, TweenSettings settings, CancellationToken token = default)
        {
            await TweenLocalScale(transform, target, settings.duration, settings.easingFunction.Resolve(), token);
        }
        
        public static async UniTask TweenLocalScale(this Transform transform, Vector3 target, 
            float duration = 1, EasingFunction easingFunction = null, CancellationToken token = default)
        {
            easingFunction ??= DefaultEasing;
            float elapsed = 0;
            Vector3 start = transform.localScale;
            
            while (elapsed < duration && !token.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                float t = easingFunction(Mathf.Clamp01(elapsed / duration));
                transform.localScale = Vector3.Lerp(start, target, t);
                await UniTask.Yield();
            }
        }
    }
}

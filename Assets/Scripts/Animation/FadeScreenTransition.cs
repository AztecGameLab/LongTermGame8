using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    public class FadeScreenTransition : MonoBehaviour ,IScreenTransition
    {
        public CanvasGroup canvasGroup;
        public float fadeDuration;

        public async UniTask Show() => await FadeTo(1);
        public async UniTask Hide() => await FadeTo(0);

        public async UniTask FadeTo(float to)
        {
            float elapsedTime = 0;
            float from = canvasGroup.alpha;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeDuration;
                canvasGroup.alpha = Mathf.Lerp(from, to, t);
                await UniTask.Yield();
            }
            
            canvasGroup.alpha = to;
        }
    }
}

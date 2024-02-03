using System.Threading;
using Cysharp.Threading.Tasks;
using Ltg8.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ltg8.Inventory
{
    public class InventoryItemUiView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TweenSettings appearTweenSettings;
        [SerializeField] private TweenSettings disappearTweenSettings;
        [SerializeField] private TweenSettings hoverStartTweenSettings;
        [SerializeField] private TweenSettings hoverStopTweenSettings;
        [SerializeField] private TweenSettings pointerDownTweenSettings;
        [SerializeField] private TweenSettings pointerUpTweenSettings;
        [SerializeField] private float hoverMultiplier = 1.25f;
        [SerializeField] private float clickMultiplier = 1.1f;
        [SerializeField] private float dragSpeed = 15f;

        private InventoryItem _item;
        private bool _isPressed;
        private bool _isDisappearing;
        private bool _isHovered;
        private Vector3 _originalScale;
        private Vector3 _targetPosition;
        private CancellationTokenSource _cts;

        public async UniTask Initialize(InventoryItem item)
        {
            _item = item;
            Transform t = transform;
            _originalScale = t.localScale;
            _targetPosition = item.position;
            t.localScale = Vector3.zero; /* start out invisible, with a scale of 0 */
            t.position = item.position; /* load the objects saved position */
            _cts = new CancellationTokenSource();
            await transform.TweenLocalScale(_originalScale, appearTweenSettings, _cts.Token); /* play an animation to become visible */
        }

        public async UniTask Disappear()
        {
            _isDisappearing = true;
            _cts?.Cancel(); _cts = null;
            await transform.TweenLocalScale(Vector3.zero, disappearTweenSettings); /* play an animation to become invisible */ 
            Destroy(gameObject);
        }
        
        private void Update()
        {
            if (_isPressed) _targetPosition = Input.mousePosition;
            Vector3 pos = Vector3.Lerp(transform.position, _targetPosition, dragSpeed * Time.deltaTime);
            transform.position = pos;
            _item.position = pos;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            if (_isDisappearing || _isPressed) return;
            _cts?.Cancel(); _cts = new CancellationTokenSource();
            transform.TweenLocalScale(_originalScale * hoverMultiplier, hoverStartTweenSettings, _cts.Token).Forget();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            if (_isDisappearing || _isPressed) return;
            _cts?.Cancel(); _cts = new CancellationTokenSource();
            transform.TweenLocalScale(_originalScale, hoverStopTweenSettings, _cts.Token).Forget();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            if (_isDisappearing) return;
            _cts?.Cancel(); _cts = new CancellationTokenSource();
            transform.TweenLocalScale(_originalScale * clickMultiplier, pointerDownTweenSettings, _cts.Token).Forget();
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            if (_isDisappearing) return;
            _cts?.Cancel(); _cts = new CancellationTokenSource();
            transform.TweenLocalScale(_isHovered ? _originalScale * hoverMultiplier : _originalScale, pointerDownTweenSettings, _cts.Token).Forget();
        }
    }
}

using System;
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

        public event Action<DropEventData> OnDrop;
        public InventoryItemData Item { get; private set; }

        private bool _isPressed;
        private bool _isDisappearing;
        private bool _isHovered;
        private Vector3 _originalScale;
        private Vector3 _targetPosition;
        private CancellationTokenSource _cts;

        private Vector3 HoveredScale => _originalScale * hoverMultiplier;
        private Vector3 ClickedScale => _originalScale * clickMultiplier;

        public async UniTask Initialize(InventoryItemData item)
        {
            Transform t = transform;

            Item = item;
            _originalScale = t.localScale;
            _targetPosition = item.position;
            _cts = new CancellationTokenSource();

            Instantiate(item.Data.uiView, t); /* spawn the ui view as a child of this object */
            t.localScale = Vector3.zero; /* start out invisible, with a scale of 0 */
            t.position = item.position; /* load the objects saved position */
            await transform.TweenLocalScale(_originalScale, appearTweenSettings, _cts.Token); /* play an animation to become visible */
        }

        public async UniTask Disappear()
        {
            _isDisappearing = true;
            CancelCurrentAnimation();
            await transform.TweenLocalScale(Vector3.zero, disappearTweenSettings); /* play an animation to become invisible */ 
            Destroy(gameObject);
        }
        
        private void Update()
        {
            if (_isPressed) _targetPosition = Input.mousePosition;
            
            // move this object towards a target position a little bit each frame
            Vector3 targetPosition = Vector3.Lerp(transform.position, _targetPosition, dragSpeed * Time.deltaTime);
            transform.position = targetPosition;
            Item.position = targetPosition;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            if (_isDisappearing || _isPressed) return; /* if we are already pressed, we don't need this animation */
            CancelCurrentAnimation();
            transform.TweenLocalScale(HoveredScale, hoverStartTweenSettings, _cts.Token).Forget(); /* play the hover animation */
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            if (_isDisappearing || _isPressed) return; /* if we are already pressed, we don't need this animation */
            CancelCurrentAnimation();
            transform.TweenLocalScale(_originalScale, hoverStopTweenSettings, _cts.Token).Forget(); /* play the un-hover animation */
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            if (_isDisappearing) return;
            CancelCurrentAnimation();
            transform.TweenLocalScale(ClickedScale, pointerDownTweenSettings, _cts.Token).Forget(); /* play the click animation */
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            if (_isDisappearing) return;
            CancelCurrentAnimation();
            Vector3 targetScale = _isHovered ? HoveredScale : _originalScale; /* we could be hovering over the object or not - choose scale accordingly */
            transform.TweenLocalScale(targetScale, pointerUpTweenSettings, _cts.Token).Forget(); /* play the un-click animation */
            OnDrop?.Invoke(new DropEventData{View = this}); /* let subscribers know that we got dropped */
        }

        private void CancelCurrentAnimation()
        {
            _cts?.Cancel(); 
            _cts = new CancellationTokenSource();
        }
        
        public struct DropEventData
        {
            public InventoryItemUiView View;
        }
    }
}

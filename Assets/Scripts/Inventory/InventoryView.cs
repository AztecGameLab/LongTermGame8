﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ltg8.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Ltg8.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private ItemTargetSelector targetSelector;
        [SerializeField] private InventoryItemUiView itemUiViewPrefab;
        [SerializeField] private TweenSettings openTween;
        [SerializeField] private TweenSettings closeTween;
        [SerializeField] private float spawnDelay = 0.1f;
        [SerializeField] private float despawnDelay = 0.1f;
        [SerializeField] private Transform itemParent;
        [SerializeField] private Volume volume;

        public UnityEvent onClose;

        private readonly List<InventoryItemUiView> _spawnedItems = new List<InventoryItemUiView>();
        private CancellationTokenSource _cts;

        public async UniTask Open(IEnumerable<InventoryItemData> items)
        {
            CancelCurrentAnimation();
            volume.TweenWeight(1, openTween, _cts.Token).Forget(); /* show the post-processing that highlights interactable objects */
            
            foreach (InventoryItemData item in items)
            {
                // spawn the object asynchronously, and save a reference to it's UiView component.
                // InventoryItemUiView uiView = Instantiate(((ItemData) item.itemData.Asset).uiView, itemParent);
                InventoryItemUiView uiView = Instantiate(itemUiViewPrefab, itemParent);
                uiView.OnDrop += HandleOnDrop;
                
                uiView.Initialize(item).Forget(); /* play some animation where the item appears */
                _spawnedItems.Add(uiView); /* keep track of all spawned items, so we can remove them later */
                await UniTask.Delay(TimeSpan.FromSeconds(spawnDelay)); /* pause a little bit between animations */
            }
        }
        
        private void HandleOnDrop(InventoryItemUiView.DropEventData eventData)
        {
            if (targetSelector.HasTarget && targetSelector.HoveredTarget.CanReceiveItem(eventData.View.Item.Data))
            {
                targetSelector.HoveredTarget.ReceiveItem(eventData.View.Item.Data);
                Ltg8.Save.Inventory.Remove(eventData.View.Item);
                eventData.View.Disappear().Forget();
            }
        }
        
        public async UniTask Close()
        {
            CancelCurrentAnimation(); 
            volume.TweenWeight(0, closeTween, _cts.Token).Forget(); /* hide the post-processing that highlights interactable objects */
            
            foreach (InventoryItemUiView item in _spawnedItems) 
            {
                item.Disappear() /* play some animation where the item disappears */
                    .ContinueWith(() => _spawnedItems.Remove(item)) /* when items are done animating, remove them from the list */
                    .Forget(); 
                await UniTask.Delay(TimeSpan.FromSeconds(despawnDelay)); /* pause a little bit between animations */
            }

            await UniTask.WaitUntil(() => _spawnedItems.Count <= 0); /* once there are no items in the list, everything has finished animating */
            onClose.Invoke();
        }
        
        private void CancelCurrentAnimation()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
        }
    }
}

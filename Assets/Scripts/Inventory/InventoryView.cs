using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private float spawnDelay = 0.1f;
        [SerializeField] private float despawnDelay = 0.1f;
        [SerializeField] private Transform itemParent;

        private readonly List<InventoryItemUiView> _spawnedItems = new List<InventoryItemUiView>();
        
        public async UniTask Open(InventoryData data)
        {
            foreach (InventoryItem item in data.Items)
            {
                // spawn the object asynchronously, and save a reference to it's UiView component.
                InventoryItemUiView uiView = (await item.uiView.InstantiateAsync(itemParent)).GetComponent<InventoryItemUiView>();
                
                uiView.Initialize(item).Forget(); /* play some animation where the item appears */
                _spawnedItems.Add(uiView); /* keep track of all spawned items, so we can remove them later */
                await UniTask.Delay(TimeSpan.FromSeconds(spawnDelay)); /* pause a little bit between animations */
            }
        }

        public async UniTask Close()
        {
            foreach (InventoryItemUiView item in _spawnedItems) 
            {
                item.Disappear() /* play some animation where the item disappears */
                    .ContinueWith(() => _spawnedItems.Remove(item)); /* when items are done animating, remove them from the list */
                await UniTask.Delay(TimeSpan.FromSeconds(despawnDelay)); /* pause a little bit between animations */
            }

            await UniTask.WaitUntil(() => _spawnedItems.Count <= 0); /* once there are no items in the list, everything has finished animating */
        }
    }
}

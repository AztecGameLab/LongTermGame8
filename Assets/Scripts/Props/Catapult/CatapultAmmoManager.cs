using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using Ltg8.Player;
using UnityEngine;

namespace Ltg8.Catapult
{
    public class CatapultAmmoManager : MonoBehaviour
    {
        public Transform loadPosition;
        public Transform unloadPosition;

        private bool _loaded;

        private GameObject _loadedItem;

        public void LoadPlayer(GameObject player)
        {
            if (_loaded) return;

            _loaded = true;

            player.transform.position = loadPosition.position;
            _loadedItem = player;
        }

        public void LoadItem(ItemData item)
        {
            if (_loaded) return;

            _loaded = true;

            UniTask<GameObject>.Awaiter createItemAwaiter =
                InventoryUtil.CreateItemInOverworld(item, loadPosition.position).GetAwaiter();
            
            createItemAwaiter.OnCompleted(() => { _loadedItem = createItemAwaiter.GetResult(); });
        }

        public void Unload()
        {
            if (_loadedItem == null) return;

            if (_loadedItem.TryGetComponent(out PlayerController player))
            {
                player.transform.position = unloadPosition.position;
            }
            else
            {
                Destroy(_loadedItem);
            }

            _loaded = false;
            _loadedItem = null;
        }

        // Also should be called for firing the catapult
        public void Reset()
        {
            _loadedItem = null;
            _loaded = false;
        }
    }
}
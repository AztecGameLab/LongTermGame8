using System.Linq.Expressions;
using Ltg8.Inventory;
using UnityEngine;

namespace Catapult
{
    public class CatapultAmmoScript : MonoBehaviour
    {
        
        [SerializeField] private GameObject launchBasketProx;
        
        [SerializeField] private Transform loadPosition;
        
        [SerializeField] private CatapultLaunchScript launchScript;
        
        private GameObject _loadedItem;
        
        private void OnGUI()
        {
            var output =
                // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                _loadedItem != null ? _loadedItem.name : "null";

            GUILayout.Label(output);
        }

        public void ObjectLoaded(GameObject enteredObject)
        {
            if (_loadedItem != null) return;
            launchScript.SetProjectile(enteredObject);
            var enteredObjectRb = enteredObject.GetComponent<Rigidbody>();
            enteredObjectRb.useGravity = false;
            enteredObjectRb.angularDrag = 0;
            enteredObject.AddComponent<TouchGrass>();
            CreateSphereCollider(enteredObject);
            _loadedItem = enteredObject;
        }

        public void ObjectUnloaded(GameObject exitedObject)
        {
            if (_loadedItem == null) return;
            if (!launchScript.GetLaunchedObject() == _loadedItem)
            {
                Invoke(nameof(RemoveItem),0.1f);
            }
            _loadedItem = null;
        }

        public void PlacePlayer(GameObject player)
        {
            if (_loadedItem) {
                _loadedItem.transform.position += new Vector3(0,-3,0);
                _loadedItem.SetActive(false);
                return;}
            if (!player.TryGetComponent(out CharacterController _)) return;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = loadPosition.position;
            launchBasketProx.SetActive(true);
            ObjectLoaded(player);
            Invoke(nameof(DelayProx),0.5f);
        }

        public void UnplacePlayer(GameObject player)
        {
            if (!player.TryGetComponent(out CharacterController playerController)) return;
            if (!playerController.enabled) return;
            launchBasketProx.SetActive(false);
            launchScript.SetProjectile(null);
        }

        public async void PlaceItem(ItemData item)
        {
            if (_loadedItem) return;

            await InventoryUtil.CreateItemInOverworld(item, loadPosition.position);
        }

        private void RemoveItem()
        {
            if (!_loadedItem || _loadedItem.TryGetComponent(out CharacterController _)) return;
            Destroy(_loadedItem);
            launchScript.SetProjectile(null);
        }
    
        private void DelayProx()
        {
            _loadedItem.GetComponent<CharacterController>().enabled = true;
        }

        private static void CreateSphereCollider(GameObject gameObject)
        {
            var enteredObjectSc = gameObject.AddComponent<SphereCollider>();
            enteredObjectSc.isTrigger = true;
            enteredObjectSc.enabled = false;
            enteredObjectSc.radius = 0.65f;
        }
    }
}
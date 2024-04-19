using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Ltg8.Inventory
{
    public abstract class ItemTarget : MonoBehaviour
    {
        public UnityEvent<ItemData> onReceiveItem;
        
        private void Start()
        {
            if (gameObject.layer != LayerMask.NameToLayer("ItemTarget"))
                Debug.LogError($"The object {name} is not on the ItemTarget layer, but has an ItemTarget script!");
        }

        public virtual bool WillConsumeItem() => true;

        public virtual bool CanReceiveItem(ItemData data)
        {
            return true;
        }

        public virtual void ReceiveItem(ItemData item)
        {
            Assert.IsTrue(CanReceiveItem(item));
            onReceiveItem.Invoke(item);
            FindAnyObjectByType<InventoryView>().Close().Forget();
        }

        private void OnDrawGizmos()
        {
            Collider col = GetComponent<Collider>();
            Color color = Color.green;
            color.a = 0.2f;
            Gizmos.color = color;
            Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        }
    }
}

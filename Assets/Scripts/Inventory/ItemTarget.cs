using UnityEngine;

namespace Ltg8.Inventory
{
    public class ItemTarget : MonoBehaviour
    {
        private void Start()
        {
            if (gameObject.layer != LayerMask.NameToLayer("ItemTarget"))
                Debug.LogError($"The object {name} is not on the ItemTarget layer, but has an ItemTarget script!");
        }

        public void ReceiveItem(ItemData item)
        {
            print($"{name} got item {item.name}");
        }
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class InventoryItemWorldDisplay : MonoBehaviour
    {
        [SerializeField] private Transform uiParent;

        private Transform _activeItem;
        private ItemData _activeData;

        public void Display(ItemData data)
        {
            if (_activeItem != null)
            {
                Destroy(_activeItem.gameObject);
                _activeItem = null;
                _activeData = null;
            }

            if (data != null)
            {
                _activeItem = Instantiate(data.uiView, uiParent).transform;
                _activeData = data;
            }
        }

        public void Collect()
        {
            if (_activeData != null)
            {
                InventoryUtil.AddItem(_activeData).Forget();
                Destroy(_activeItem.gameObject);
                _activeItem = null;
                _activeData = null;
            }
        }
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class InventoryItemWorldDisplay : MonoBehaviour
    {
        [SerializeField] private Transform uiParent;
        [SerializeField] private string defaultItemId;

        private Transform _activeItem;
        private ItemData _activeData;

        private void Start()
        {
            if (defaultItemId != string.Empty)
                Display(Ltg8.ItemRegistry.FindItem(defaultItemId));
        }

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
                InventoryUtil.AddItem(_activeData.guid).Forget();
                Destroy(_activeItem.gameObject);
                _activeItem = null;
                _activeData = null;
            }
        }
    }
}

using UnityEngine;

namespace Ltg8.Inventory
{
    public class InventoryItemWorldDisplay : MonoBehaviour
    {
        [SerializeField] private Transform uiParent;

        private Transform _activeItem;
        public ItemData CurrentData { get; private set; }

        public void Display(ItemData data)
        {
            CurrentData = data;
            
            if (_activeItem != null)
            {
                Destroy(_activeItem.gameObject);
                _activeItem = null;
            }
            
            if (data != null) 
                _activeItem = Instantiate(data.uiView, uiParent).transform;
        }
    }
}

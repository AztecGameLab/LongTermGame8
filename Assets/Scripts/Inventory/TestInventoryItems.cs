using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class TestInventoryItems : MonoBehaviour
    {
        public List<InventoryItem> items = new List<InventoryItem>();
        public InventoryView inventoryView;

        private void Start()
        {
            Ltg8.Save.Inventory.Items = items;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Open")) inventoryView.Open(Ltg8.Save.Inventory).Forget();
            if (GUILayout.Button("Close")) inventoryView.Close().Forget();
        }
    }
}

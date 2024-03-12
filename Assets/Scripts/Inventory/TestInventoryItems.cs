using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class TestInventoryItems : MonoBehaviour
    {
        public List<InventoryItemData> items = new List<InventoryItemData>();
        public ItemData testCollectData;
        public InventoryView inventoryView;
        public ItemCollectedAnimation collectedAnimation;

        private void Start()
        {
            Ltg8.Save.Inventory = items;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Open")) inventoryView.Open(Ltg8.Save.Inventory).Forget();
            if (GUILayout.Button("Close")) inventoryView.Close().Forget();
            if (GUILayout.Button("Give Item")) InventoryUtil.AddItem(testCollectData.guid).Forget();
            if (GUILayout.Button("Drop Item"))
            {
                InventoryItemData item = Ltg8.Save.Inventory[0];
                InventoryUtil.CreateItemInOverworld(item.Data, new Vector3(1, 1, 1));
                Ltg8.Save.Inventory.Remove(item);
            }
        }
    }
}

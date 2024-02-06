using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class TestInventoryItems : MonoBehaviour
    {
        public List<InventoryItem> items = new List<InventoryItem>();
        public ItemData testCollectData;
        public InventoryView inventoryView;
        public ItemCollectedAnimation collectedAnimation;

        private void Start()
        {
            Ltg8.Save.Inventory.Items = items;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Open")) inventoryView.Open(Ltg8.Save.Inventory).Forget();
            if (GUILayout.Button("Close")) inventoryView.Close().Forget();
            if (GUILayout.Button("Give Item")) collectedAnimation.Play(new InventoryItem{position = new Vector2(100, 100), Data = testCollectData}).Forget();
        }
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8.Inventory
{
    public static class InventoryUtil
    {
        public static async UniTask AddItem(string itemId)
        {
            await Object.FindAnyObjectByType<ItemCollectedAnimation>().Play(new InventoryItemData {
                position = new Vector2(100, 100),
                Data = Ltg8.ItemRegistry.FindItem(itemId),
            });
        }

        public static void RemoveItem(string itemId)
        {
            foreach (InventoryItemData itemData in Ltg8.Save.Inventory)
            {
                if (itemData.Data.guid == itemId)
                {
                    Ltg8.Save.Inventory.Remove(itemData);
                    return;
                }
            }
        }

        public static InventoryItemWorldDisplay CreateItemInOverworld(ItemData data, Vector3 position)
        {
            InventoryItemWorldDisplay instance = Object.Instantiate(Ltg8.Settings.overworldItemPrefab, position, Quaternion.identity);
            instance.Display(data);
            return instance;
        }
    }
}

using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using UnityEngine;

namespace Inventory
{
    // NOTE: A static class cannot be instantiated, methods can only be accessed through the class itself
    public static class InventoryUtil
    {
        // NOTE: Since the class is static, all methods should also be static
        // This methods adds an item into the player's inventory
        public static async UniTask AddItem(string itemId)
        {
            // NOTE: The await keyword will wait until the following code has completed
            // 
            await Object.FindAnyObjectByType<ItemCollectedAnimation>().Play(new InventoryItemData {
                position = new Vector2(100, 100),
                Data = Ltg8.Ltg8.ItemRegistry.FindItem(itemId),
            });
        }

        public static void RemoveItem(string itemId)
        {
            foreach (InventoryItemData itemData in Ltg8.Ltg8.Save.Inventory)
            {
                if (itemData.Data.guid == itemId)
                {
                    Ltg8.Ltg8.Save.Inventory.Remove(itemData);
                    return;
                }
            }
        }

        public static InventoryItemWorldDisplay CreateItemInOverworld(ItemData data, Vector3 position)
        {
            InventoryItemWorldDisplay instance = Object.Instantiate(Ltg8.Ltg8.Settings.overworldItemPrefab, position, Quaternion.identity);
            instance.Display(data);
            return instance;
        }
    }
}

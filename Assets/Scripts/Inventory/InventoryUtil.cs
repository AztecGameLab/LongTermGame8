using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

        public static async UniTask<GameObject> CreateItemInOverworld(ItemData data, Vector3 position)
        {
            GameObject instance = await Addressables.InstantiateAsync("prefabs/overworld_item", position, Quaternion.identity);
            instance.GetComponentInChildren<InventoryItemWorldDisplay>().Display(data);
            return instance;
        }
    }
}

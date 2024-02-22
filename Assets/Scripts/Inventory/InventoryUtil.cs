using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Ltg8.Inventory
{
    public static class InventoryUtil
    {
        public static async UniTask AddItem(ItemData data)
        {
            await Object.FindAnyObjectByType<ItemCollectedAnimation>().Play(new InventoryItemData {
                position = new Vector2(100, 100),
                Data = data,
            });
        }

        public static async UniTask<GameObject> CreateItemInOverworld(ItemData data, Vector3 position)
        {
            GameObject instance = await Addressables.InstantiateAsync("prefabs/overworld_item", position, Quaternion.identity);
            instance.GetComponentInChildren<InventoryItemWorldDisplay>().Display(data);
            return instance;
        }
    }
}

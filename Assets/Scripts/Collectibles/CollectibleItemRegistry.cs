using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Collectibles
{
    // [CreateAssetMenu(fileName = "New Collectible Item Registry", menuName = "Collectible Item Registry", order = 0)]
    public class CollectibleItemRegistry : ScriptableObject
    {
        private const string COLLECTIBLE_DIRECTORY = "Assets/Collectibles";

        [MenuItem("Tools/LTG8/Auto Register Items")]
        public static void RegisterItems()
        {
            string[] registryGuids =
                AssetDatabase.FindAssets("t:CollectibleItemRegistry", new[] { COLLECTIBLE_DIRECTORY });
            if (registryGuids.Length <= 0)
                return;

            CollectibleItemRegistry registry =
                AssetDatabase.LoadAssetAtPath<CollectibleItemRegistry>(AssetDatabase.GUIDToAssetPath(registryGuids[0]));

            string[] guids =
                AssetDatabase.FindAssets("t:InventoryCollectibleItem", new[] { COLLECTIBLE_DIRECTORY });

            registry.items.Clear();
            foreach (var guid in guids)
            {
                registry.items.Add(AssetDatabase.LoadAssetAtPath<InventoryCollectibleItem>(AssetDatabase.GUIDToAssetPath(guid)));
            }

        }

        public List<InventoryCollectibleItem> items;
    }
}
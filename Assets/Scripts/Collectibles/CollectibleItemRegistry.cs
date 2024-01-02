using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Collectibles
{
    // [CreateAssetMenu(fileName = "New Collectible Item Registry", menuName = "Collectible Item Registry", order = 0)]
    public class CollectibleItemRegistry : ScriptableObject
    {
        private const string COLLECTIBLE_DIRECTORY = "Assets/Collectibles";

        [MenuItem("LTG8/Auto Register Items")]
        public static void RegisterItems()
        {
            if (!EditorUtility.DisplayDialog("Reload Item Registry?",
                    "Are you sure you want to reload the registry?\n\nThis will clear the registered items and insert every item from the `" +
                    COLLECTIBLE_DIRECTORY + "` directory.", "Yes", "No"))
                return;

            string[] registryGuids =
                AssetDatabase.FindAssets("t:CollectibleItemRegistry", new[] { COLLECTIBLE_DIRECTORY });
            if (registryGuids.Length <= 0)
                return;

            CollectibleItemRegistry registry =
                AssetDatabase.LoadAssetAtPath<CollectibleItemRegistry>(AssetDatabase.GUIDToAssetPath(registryGuids[0]));

            string[] guids =
                AssetDatabase.FindAssets("t:InventoryCollectibleItem", new[] { COLLECTIBLE_DIRECTORY });

            registry.items.Clear();

            Dictionary<string, List<InventoryCollectibleItem>> items =
                new Dictionary<string, List<InventoryCollectibleItem>>();

            foreach (var guid in guids)
            {
                InventoryCollectibleItem item =
                    AssetDatabase.LoadAssetAtPath<InventoryCollectibleItem>(AssetDatabase.GUIDToAssetPath(guid));

                List<InventoryCollectibleItem> itemsWithId =
                    items.GetValueOrDefault(item.itemId, new List<InventoryCollectibleItem>());
                itemsWithId.Add(item);
                items[item.itemId] = itemsWithId;

                registry.items.Add(item);
            }

            if (items.Values.Any(list => list.Count > 1))
            {
                StringBuilder message =
                    new StringBuilder(
                        "Some Item IDs are used more than once, which could cause issues with saving and loading data.\n\n");
                
                foreach (var idPair in items.Where(idPair => idPair.Value.Count > 1))
                {
                    message.AppendFormat("`{0}` id used in: ", idPair.Key);
                    foreach (var item in idPair.Value)
                    {
                        message.AppendFormat("\n  - {0}", item.name);
                    }

                    message.Append("\n\n");
                }

                message.Remove(message.Length-2, 2);

                EditorUtility.DisplayDialog("!!WARNING!! Duplicate Item IDs", message.ToString(), "Ok");
            }
        }

        public List<InventoryCollectibleItem> items;
    }
}
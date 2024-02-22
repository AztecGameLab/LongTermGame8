using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Ltg8.Inventory
{
    [Serializable]
    public class InventoryItemData
    {
        public Vector2 position;
        public AssetReference itemData;
        
        public ItemData Data 
        {
            get
            {
                if (itemData.Asset == null)
                    itemData.LoadAssetAsync<ItemData>().WaitForCompletion();
                
                return (ItemData)itemData.Asset;
            }
            set => itemData = new AssetReference(value.guid);
        }
    }
}

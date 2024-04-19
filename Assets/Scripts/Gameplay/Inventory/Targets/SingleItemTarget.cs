using UnityEngine;

namespace Ltg8.Inventory
{
    public class SingleItemTarget : ItemTarget
    {
        [SerializeField] private string expectedItemGuid;

        public override bool CanReceiveItem(ItemData data)
        {
            return data.guid == expectedItemGuid;
        }
    }
}

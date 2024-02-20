using UnityEngine;

namespace Ltg8.Inventory
{
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        public string guid;
        public string itemName;
        public GameObject uiView;
    }
}

using System;
using Misc;
using UnityEngine;

namespace Ltg8.Inventory
{
    public class ItemTargetSelector : MonoBehaviour
    {
        public GameObject HoveredTarget { get; private set; }
        public bool HasTarget { get; private set; }
        public event Action<ItemTargetChangeEventData> OnTargetChange; 

        private readonly RaycastHit[] _resultBuffer = new RaycastHit[15];
        private GameObject _prevTarget;

        private void Update()
        {
            _prevTarget = HoveredTarget;
            Ray ray = Ltg8.MainCamera.ScreenPointToRay(Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(ray, _resultBuffer, float.PositiveInfinity, LayerMask.GetMask("ItemTarget"));
            if (HoveredTarget)
            {
                HoveredTarget = hits == 0
                    ? null
                    : RaycastUtil.FindNearestWithComponent<ItemTarget>(hits, _resultBuffer).gameObject;
            }

            if (_prevTarget != HoveredTarget)
            {
                HasTarget = HoveredTarget != null;
                OnTargetChange?.Invoke(new ItemTargetChangeEventData{OldTarget = _prevTarget, NewTarget = HoveredTarget});
            }
        }
    }
    
    public struct ItemTargetChangeEventData
    {
        public GameObject NewTarget;
        public GameObject OldTarget;
    }
}

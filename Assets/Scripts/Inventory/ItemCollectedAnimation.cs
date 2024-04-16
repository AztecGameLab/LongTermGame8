using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Ltg8.Inventory
{
    public class ItemCollectedAnimation : MonoBehaviour
    {
        [SerializeField] private PlayableDirector timeline;
        [SerializeField] private Transform uiViewParent;
        [SerializeField] private TMP_Text nameText;

        private GameObject _prevInstance;
        
        public async UniTask Play(InventoryItemData item)
        {
            if (_prevInstance != null) Destroy(_prevInstance.gameObject);
            nameText.SetText(item.Data.itemName);
            _prevInstance = Instantiate(item.Data.uiView, uiViewParent);
            GetComponent<CanvasGroup>().alpha = 1;
            transform.localScale = Vector3.one;
            timeline.Play();
            Ltg8.Save.Inventory.Add(item);
            await UniTask.WaitUntil(() => timeline.state != PlayState.Playing);
            await UniTask.WaitUntil(() => Input.anyKeyDown);
            timeline.Resume();
            await UniTask.WaitUntil(() => timeline.state != PlayState.Playing);
        }
    }
}

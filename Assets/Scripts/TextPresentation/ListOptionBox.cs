using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ltg8
{
    public class ListOptionBox : OptionBoxView
    {
        [SerializeField]
        private Transform optionParent;
        
        [SerializeField]
        private GameObject selectedOptionIcon;
        
        private int _selectedOption;

        protected override void Awake()
        {
            base.Awake();
            
            for (int i = 0; i < optionParent.childCount; i++)
                Destroy(optionParent.GetChild(i).gameObject);
        }

        public override async UniTask<int> PickOption(OptionData first, OptionData second)
        {
            SingleOptionView op1 = CreateOption(first, 0);
            SingleOptionView op2 = CreateOption(second, 1);
            int result = await WaitForSelection(op1);
            ReleaseOption(op1);
            ReleaseOption(op2);
            return result;
        }
        
        public override async UniTask<int> PickOption(OptionData first, OptionData second, OptionData third)
        {
            SingleOptionView op1 = CreateOption(first, 0);
            SingleOptionView op2 = CreateOption(second, 1);
            SingleOptionView op3 = CreateOption(third, 2);
            int result = await WaitForSelection(op1);
            ReleaseOption(op1);
            ReleaseOption(op2);
            ReleaseOption(op3);
            return result;
        }
        
        public override async UniTask<int> PickOption(OptionData first, OptionData second, OptionData third, OptionData fourth)
        {
            SingleOptionView op1 = CreateOption(first, 0);
            SingleOptionView op2 = CreateOption(second, 1);
            SingleOptionView op3 = CreateOption(third, 2);
            SingleOptionView op4 = CreateOption(fourth, 3);
            int result = await WaitForSelection(op1);
            ReleaseOption(op1);
            ReleaseOption(op2);
            ReleaseOption(op3);
            ReleaseOption(op4);
            return result;
        }
        
        private SingleOptionView CreateOption(OptionData data, int id)
        {
            SingleOptionView option = OptionPool.Get();
            option.gameObject.SetActive(true);
            option.transform.SetParent(optionParent);
            
            // stupid TMP trying to be smart, let me pool stuff!
            option.textDisplay.autoSizeTextContainer = false;
            option.textDisplay.autoSizeTextContainer = true;
            
            option.textDisplay.SetText(data.message);
            option.onSelect.AddListener(() => _selectedOption = id);
            option.onHover.AddListener(() => {
                Animation = data.Animation;
                RuntimeManager.PlayOneShot(optionHoverSfx);
            });
            return option;
        }

        private void ReleaseOption(SingleOptionView option)
        {
            option.onSelect.RemoveAllListeners();
            option.onHover.RemoveAllListeners();
            option.gameObject.SetActive(false);
            OptionPool.Release(option);
        }

        private async UniTask<int> WaitForSelection(SingleOptionView initialSelection)
        {
            _selectedOption = -1;
            EventSystem.current.SetSelectedGameObject(initialSelection.textDisplay.gameObject);
            
            while (_selectedOption == -1)
            {
                // Make sure the selection hint follows the current selection
                Transform s = EventSystem.current.currentSelectedGameObject.transform;
                Vector3 pos = s.position;
                pos.x += ((RectTransform)s).rect.xMax; // align with the right edge of text
                selectedOptionIcon.transform.position = pos;
                
                await UniTask.Yield();
            }

            RuntimeManager.PlayOneShot(optionSelectSfx);
            return _selectedOption;
        }
    }
}

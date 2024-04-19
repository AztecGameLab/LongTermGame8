using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ltg8
{
    public class RadialOptionBox : OptionBoxView
    {
        [SerializeField] 
        private List<RadialOptionView> pickTwoOptions;
        
        [SerializeField] 
        private List<RadialOptionView> pickThreeOptions;
        
        [SerializeField] 
        private List<RadialOptionView> pickFourOptions;

        private int _selectedOption;
        private RadialOptionView _hoveredOption;

        protected override void Awake()
        {
            base.Awake();
            
            if (pickTwoOptions.Count != 2) Debug.LogError("pickTwoOptions must have exactly two values!");
            if (pickThreeOptions.Count != 3) Debug.LogError("pickThreeOptions must have exactly three values!");
            if (pickFourOptions.Count != 4) Debug.LogError("pickFourOptions must have exactly four values!");

            foreach (RadialOptionView option in pickTwoOptions) option.gameObject.SetActive(false);
            foreach (RadialOptionView option in pickThreeOptions) option.gameObject.SetActive(false);
            foreach (RadialOptionView option in pickFourOptions) option.gameObject.SetActive(false);
        }
        
        public override async UniTask<int> PickOption(OptionData first, OptionData second)
        {
            SetSelectedOption(pickTwoOptions[0]);
            SetupOption(pickTwoOptions[0], first, 0);
            SetupOption(pickTwoOptions[1], second, 1);
            int result = await WaitForSelection();
            ReleaseOptions(pickTwoOptions);
            return result;
        }
        
        public override async UniTask<int> PickOption(OptionData first, OptionData second, OptionData third)
        {
            SetSelectedOption(pickThreeOptions[1]);
            SetupOption(pickThreeOptions[0], first, 0);
            SetupOption(pickThreeOptions[1], second, 1);
            SetupOption(pickThreeOptions[2], third, 2);
            int result = await WaitForSelection();
            ReleaseOptions(pickThreeOptions);
            return result;
        }
        
        public override async UniTask<int> PickOption(OptionData first, OptionData second, OptionData third, OptionData fourth)
        {
            SetSelectedOption(pickFourOptions[1]);
            SetupOption(pickFourOptions[0], first, 0);
            SetupOption(pickFourOptions[1], second, 1);
            SetupOption(pickFourOptions[2], third, 2);
            SetupOption(pickFourOptions[3], fourth, 3);
            int result = await WaitForSelection();
            ReleaseOptions(pickFourOptions);
            return result;
        }

        private void SetSelectedOption(RadialOptionView view)
        {
            EventSystem.current.SetSelectedGameObject(view.optionView.button.gameObject);
            _hoveredOption = view;
        }
        
        private void SetupOption(RadialOptionView option, OptionData data, int id)
        {
            option.gameObject.SetActive(true);
            option.optionView.textDisplay.SetText(data.message);
            option.optionView.onSelect.AddListener(() => {
                RuntimeManager.PlayOneShot(optionSelectSfx);
                _selectedOption = id;
            });
            option.optionView.onHover.AddListener(() => {
                RuntimeManager.PlayOneShot(optionHoverSfx);
                _hoveredOption.selectionHint.SetActive(false);
                _hoveredOption = option;
                _hoveredOption.selectionHint.SetActive(true);
                Animation = data.Animation;
            });
        }

        private void ReleaseOptions(List<RadialOptionView> options)
        {
            foreach (RadialOptionView option in options)
            {
                option.gameObject.SetActive(false);
                option.selectionHint.SetActive(false);
                option.optionView.onSelect.RemoveAllListeners();
                option.optionView.onHover.RemoveAllListeners();
            }
        }
        
        private async UniTask<int> WaitForSelection()
        {
            _selectedOption = -1;

            while (_selectedOption == -1)
                await UniTask.Yield();
            
            return _selectedOption;
        }
    }
}

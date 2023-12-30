using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

// todo: cleanup

namespace Ltg8
{
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [SerializeField] private Image normalFrame;
        [SerializeField] private Image optionsFrame;
        
        [SerializeField] 
        private ImageFlipBookView textImage;
        
        [SerializeField] 
        private CanvasGroup textBoxGraphics;
        
        [SerializeField] 
        private TMP_Text textBoxText;
        
        [SerializeField] 
        private GameObject continueHint;

        [SerializeField]
        private GameObject selectedOptionIcon;

        [SerializeField]
        private OptionView optionPrefab;

        [SerializeField] 
        private CanvasGroup optionBoxGraphics;

        [SerializeField] 
        private CanvasGroup nameBoxGraphics;
        
        [SerializeField] 
        private TMP_Text nameBoxText;
        
        [SerializeField] 
        private float openDuration;
        
        [SerializeField] 
        private float closeDuration;

        [SerializeField]
        private float clearDuration;
        
        [SerializeField] 
        private int revealIntervalMs;
        
        [SerializeField] 
        private EventReference revealChirp;
        
        [SerializeField] 
        private EventReference confirmChirp;

        [SerializeField] 
        private EventReference optionSelectChirp;
        
        [SerializeField] 
        private EventReference optionHoverChirp;

        private IFlipBookAnimation _animation;
        private bool _continueRequested;
        private ObjectPool<OptionView> _optionPool;
        
        private void Start()
        {
            _optionPool = new ObjectPool<OptionView>(() => {
                OptionView instance = Instantiate(optionPrefab, optionBoxGraphics.transform);
                instance.gameObject.SetActive(false);
                return instance;
            });

            normalFrame.gameObject.SetActive(true);
            optionsFrame.gameObject.SetActive(false);
            textImage.gameObject.SetActive(false);
            continueHint.SetActive(false);
            nameBoxGraphics.gameObject.SetActive(false);
            optionBoxGraphics.gameObject.SetActive(false);
            selectedOptionIcon.gameObject.SetActive(false);
            textBoxText.text = string.Empty;
            textBoxText.maxVisibleCharacters = 0;
        }

        private void Update()
        {
            if (_animation != null)
                _animation.UpdateOn(textImage, Time.deltaTime);
        }

        public UniTask ShowAnimation(IFlipBookAnimation anim)
        {
            textImage.gameObject.SetActive(true);
            _animation = anim;
            _animation.UpdateOn(textImage, Time.deltaTime);
            return UniTask.CompletedTask;
        }

        public UniTask HideAnimation()
        {
            _animation = null;
            textImage.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        private async UniTask FadeCanvasGroup(CanvasGroup canvasGroup, float from, float to, float duration)
        {
            float elapsed = 0;
            
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                await UniTask.Yield();
            }

            canvasGroup.alpha = to;
        }
        
        public async UniTask Open()
        {
            await FadeCanvasGroup(textBoxGraphics, 0, 1, openDuration);
        }

        public async UniTask Open(string displayName)
        {
            nameBoxText.text = displayName;
            nameBoxGraphics.gameObject.SetActive(true);
            await Open();
        }
        
        public async UniTask Close()
        {
            await FadeCanvasGroup(textBoxGraphics, 1, 0, closeDuration);
        }

        public async UniTask ShowName()
        {
            nameBoxGraphics.gameObject.SetActive(true);
            await FadeCanvasGroup(nameBoxGraphics, 0, 1, openDuration);
        }

        public async UniTask HideName()
        {
            await FadeCanvasGroup(nameBoxGraphics, 1, 0, closeDuration);
            nameBoxGraphics.gameObject.SetActive(false);
        }

        public UniTask SetName(string value)
        {
            nameBoxText.text = value;
            return UniTask.CompletedTask;
        }

        public async UniTask WaitForContinue()
        {
            _continueRequested = false;
            continueHint.SetActive(true);
            
            // Will not finish until `HandleContinue` is called.
            while (!_continueRequested)
                await UniTask.Yield();
            
            RuntimeManager.PlayOneShot(confirmChirp);
            continueHint.SetActive(false);
            _continueRequested = false;
        }
        
        public void HandleContinue(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Called by some UnityEvent or outside script
                // to signal the player is ready to read more text.
                _continueRequested = true;
            }
        }

        public async UniTask ClearText()
        {
            textBoxText.text = string.Empty;
            textBoxText.maxVisibleCharacters = 0;
            await UniTask.Delay(TimeSpan.FromSeconds(clearDuration));
        }

        public async UniTask Delay(float seconds)
        {
            float elapsed = 0;

            while (elapsed < seconds && !_continueRequested)
            {
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }
        }
        
        public async UniTask WriteText(string text)
        {
            textBoxText.text += text;

            int processedCharacters = 0;
            int totalCharacters = text.Length;
            
            while (processedCharacters < totalCharacters)
            {
                // If the player wants to continue, we immediately show the rest of
                // the text (the fast-reader button).
                if (_continueRequested)
                {
                    textBoxText.maxVisibleCharacters += totalCharacters - processedCharacters;
                    RuntimeManager.PlayOneShot(revealChirp);
                    break;
                }
                        
                // We only want to chirp on visible characters, e.g. anything BUT a space
                if (text[processedCharacters] != ' ')
                    RuntimeManager.PlayOneShot(revealChirp);
                        
                processedCharacters++;
                textBoxText.maxVisibleCharacters++;
                await UniTask.Delay(revealIntervalMs);
            }
        }

        public OptionBuilder PrepareOptions()
        {
            return new OptionBuilder(this);
        }

        public class OptionBuilder
        {
            private readonly TextBoxPresenter _presenter;
            private readonly List<OptionView> _views = new List<OptionView>();

            private int _selectedOption = -1;
            private int _hoveredOption;
            
            public OptionBuilder(TextBoxPresenter presenter)
            {
                _presenter = presenter;
            }
            
            public async UniTask<int> Present()
            {
                foreach (OptionView view in _views)
                    view.textDisplay.autoSizeTextContainer = true;
                
                _presenter.selectedOptionIcon.SetActive(true);
                _presenter.optionBoxGraphics.gameObject.SetActive(true);
                
                // set initial pos of selection hint
                {
                    TMP_Text s = _views[_hoveredOption].textDisplay;
                    _presenter.selectedOptionIcon.transform.position = new Vector3(s.transform.position.x + ((RectTransform) s.transform).rect.xMax, s.transform.position.y, s.transform.position.z);
                }

                _presenter.normalFrame.gameObject.SetActive(false);
                _presenter.optionsFrame.gameObject.SetActive(true);
                
                // Wait until an option is selected.
                while (_selectedOption == -1)
                {
                    TMP_Text s = _views[_hoveredOption].textDisplay;
                    _presenter.selectedOptionIcon.transform.position = new Vector3(s.transform.position.x + ((RectTransform) s.transform).rect.xMax, s.transform.position.y, s.transform.position.z);
                    await UniTask.Yield();
                }

                foreach (OptionView view in _views)
                    view.enabled = false;
                
                RuntimeManager.PlayOneShot(_presenter.optionSelectChirp);
                _presenter.normalFrame.gameObject.SetActive(true);
                _presenter.optionsFrame.gameObject.SetActive(false);
                _presenter.optionBoxGraphics.gameObject.SetActive(false);
                _presenter.selectedOptionIcon.gameObject.SetActive(false);
                
                // Release all the borrowed options
                foreach (OptionView view in _views)
                {
                    view.gameObject.SetActive(false);
                    view.onSelect.RemoveAllListeners();
                    view.onHover.RemoveAllListeners();
                    _presenter._optionPool.Release(view);
                }
                
                // We always want a clean slate after making a decision
                await _presenter.ClearText();
                return _selectedOption;
            }

            public OptionBuilder With(string text)
            {
                int index = _views.Count;
                OptionView view = _presenter._optionPool.Get();
                view.gameObject.SetActive(true);
                view.textDisplay.SetText(text);
                view.onSelect.AddListener(() => _selectedOption = index);
                view.onHover.AddListener(() => {
                    RuntimeManager.PlayOneShot(_presenter.optionHoverChirp);
                    _hoveredOption = index;
                });
                _views.Add(view);
                return this;
            }
        }
    }
}

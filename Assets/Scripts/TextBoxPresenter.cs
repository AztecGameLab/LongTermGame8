using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

// todo: cleanup
// todo: create a 'view' object for the text box, to replace the 'style' images (more flexible)
// todo: selection arrow need a bit more work too
// todo: more randomization on the chirps (ref celeste, talk w/ luke)

namespace Ltg8
{
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [Title("Styles")]
        
        [SerializeField]
        private TextBoxStyle defaultTextBoxStyle;
        
        [SerializeField] 
        private RevealStyle defaultRevealStyle;
        
        [Title("References")]
        
        [SerializeField]
        private Image normalFrame;
        
        [SerializeField]
        private Image optionsFrame;
        
        [SerializeField]
        private Image selectedOptionIcon;
        
        [SerializeField] 
        private Image continueHint;
        
        [SerializeField] 
        private RawImageFlipBookView textRawImage;
        
        [SerializeField] 
        private CanvasGroup textBoxGraphics;
        
        [SerializeField] 
        private CanvasGroup optionBoxGraphics;

        [SerializeField] 
        private CanvasGroup nameBoxGraphics;
        
        [SerializeField] 
        private TMP_Text textBoxText;
        
        [SerializeField] 
        private TMP_Text nameBoxText;
        
        [SerializeField]
        private OptionView optionPrefab;
        
        [Title("Audio")]
        
        [SerializeField] 
        private EventReference confirmChirp;

        [SerializeField] 
        private EventReference optionSelectChirp;
        
        [SerializeField] 
        private EventReference optionHoverChirp;
        
        [Title("Settings")]

        [SerializeField] 
        private float openDuration;
        
        [SerializeField] 
        private float closeDuration;

        [SerializeField]
        private float clearDuration;
        
        private IFlipBookAnimation _animation;
        private bool _continueRequested;
        private ObjectPool<OptionView> _optionPool;
        private Stack<RevealStyle> _revealStyleStack;
        private Stack<TextBoxStyle> _textBoxStyleStack;

        private RevealStyle RevealStyle => _revealStyleStack.Peek();

        public void PushRevealStyle(RevealStyle style)
        {
            _revealStyleStack.Push(style);
        }

        public void PopRevealStyle()
        {
            _revealStyleStack.Pop();
        }

        public void PushTextBoxStyle(TextBoxStyle style)
        {
            _textBoxStyleStack.Push(style);
            ApplyCurrentTextBoxStyle();
        }

        public void PopTextBoxStyle()
        {
            _textBoxStyleStack.Pop();
            ApplyCurrentTextBoxStyle();
        }

        private void ApplyCurrentTextBoxStyle()
        {
            TextBoxStyle s = _textBoxStyleStack.Peek();
            normalFrame.sprite = s.normalFrame;
            optionsFrame.sprite = s.optionsFrame;
            selectedOptionIcon.sprite = s.selectedOptionIcon;
            continueHint.sprite = s.continueHint;
        }
        
        private void Start()
        {
            _optionPool = new ObjectPool<OptionView>(() => {
                OptionView instance = Instantiate(optionPrefab, optionBoxGraphics.transform);
                instance.gameObject.SetActive(false);
                return instance;
            });

            _revealStyleStack = new Stack<RevealStyle>();
            _revealStyleStack.Push(defaultRevealStyle);
            _textBoxStyleStack = new Stack<TextBoxStyle>();
            _textBoxStyleStack.Push(defaultTextBoxStyle);
            
            normalFrame.gameObject.SetActive(true);
            optionsFrame.gameObject.SetActive(false);
            textRawImage.gameObject.SetActive(false);
            continueHint.gameObject.SetActive(false);
            nameBoxGraphics.gameObject.SetActive(false);
            optionBoxGraphics.gameObject.SetActive(false);
            selectedOptionIcon.gameObject.SetActive(false);
            
            textBoxText.text = string.Empty;
            textBoxText.maxVisibleCharacters = 0;
        }

        private void Update()
        {
            if (_animation != null)
                _animation.UpdateOn(textRawImage, Time.deltaTime);
        }

        public UniTask ShowAnimation(IFlipBookAnimation anim)
        {
            textRawImage.gameObject.SetActive(true);
            _animation = anim;
            _animation.UpdateOn(textRawImage, Time.deltaTime);
            return UniTask.CompletedTask;
        }

        public UniTask HideAnimation()
        {
            _animation = null;
            textRawImage.gameObject.SetActive(false);
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
            continueHint.gameObject.SetActive(true);
            
            // Will not finish until `HandleContinue` is called.
            while (!_continueRequested)
                await UniTask.Yield();
            
            RuntimeManager.PlayOneShot(confirmChirp);
            continueHint.gameObject.SetActive(false);
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
                    RuntimeManager.PlayOneShot(RevealStyle.audioPerCharacter);
                    break;
                }
                        
                // We only want to chirp on visible characters, e.g. anything BUT a space
                if (text[processedCharacters] != ' ')
                    RuntimeManager.PlayOneShot(RevealStyle.audioPerCharacter);
                        
                processedCharacters++;
                textBoxText.maxVisibleCharacters++;
                await UniTask.Delay(RevealStyle.revealIntervalMs);
            }
        }

        public OptionBuilder PrepareDecision()
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
                
                _presenter.selectedOptionIcon.gameObject.SetActive(true);
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

            public OptionBuilder WithOption(string text)
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

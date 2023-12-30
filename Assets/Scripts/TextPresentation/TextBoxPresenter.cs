using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

// todo: cleanup
// todo: more randomization on the chirps (ref celeste, talk w/ luke)
// todo: better test w/ fake convo

namespace Ltg8
{
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [Title("Styles")]
        
        [SerializeField]
        private TextBoxView defaultTextBoxView;
        
        [SerializeField] 
        private RevealStyle defaultRevealStyle;
        
        [Title("Audio")]
        
        [SerializeField] 
        private EventReference confirmChirp;

        [SerializeField] 
        private EventReference optionSelectChirp;
        
        [SerializeField] 
        private EventReference optionHoverChirp;
        
        [Title("Settings")]

        [SerializeField]
        private float clearDuration;
        
        private IFlipBookAnimation _animation;
        private bool _continueRequested;
        private ObjectPool<OptionView> _optionPool;
        private RevealStyle _revealStyle;
        private TextBoxView _textBoxView;

        public void SetRevealStyle(RevealStyle style)
        {
            _revealStyle = style;
        }

        public void SetTextBoxView(TextBoxView view)
        {
            _textBoxView.gameObject.SetActive(false);
            _textBoxView = view;
            _textBoxView.Initialize();
            _textBoxView.gameObject.SetActive(true);
        }

        public void DefaultTextBoxView()
        {
            SetTextBoxView(defaultTextBoxView);
        }

        public void DefaultRevealStyle()
        {
           SetRevealStyle(defaultRevealStyle);
        }

        private void Start()
        {
            _optionPool = new ObjectPool<OptionView>(() => {
                OptionView instance = Instantiate(_textBoxView.optionPrefab, _textBoxView.optionParent.transform);
                instance.gameObject.SetActive(false);
                return instance;
            });
            
            DefaultRevealStyle();
            
            _textBoxView = defaultTextBoxView;
            _textBoxView.Initialize();
            _textBoxView.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_animation != null)
            {
                _animation.Update(Time.deltaTime);
                _animation.ApplyTo(_textBoxView.normalAnimationImage);
            }
        }

        public UniTask ShowAnimation(IFlipBookAnimation anim)
        {
            _textBoxView.normalAnimationImage.gameObject.SetActive(true);
            _animation = anim;
            _animation.ApplyTo(_textBoxView.normalAnimationImage);
            return UniTask.CompletedTask;
        }

        public UniTask HideAnimation()
        {
            _animation = null;
            _textBoxView.normalAnimationImage.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public UniTask Open()
        {
            _textBoxView.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public async UniTask Open(string displayName)
        {
            SetName(displayName);
            ShowName();
            await Open();
        }
        
        public UniTask Close()
        {
            _textBoxView.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public void ShowName()
        {
            _textBoxView.nameBoxFrame.SetActive(true);
        }

        public void HideName()
        {
            _textBoxView.nameBoxFrame.SetActive(true);
        }

        public void SetName(string value)
        {
            _textBoxView.nameBoxText.SetText(value);
        }

        public async UniTask WaitForContinue()
        {
            _continueRequested = false;
            _textBoxView.continueHint.gameObject.SetActive(true);
            
            // Will not finish until `HandleContinue` is called.
            while (!_continueRequested)
                await UniTask.Yield();
            
            RuntimeManager.PlayOneShot(confirmChirp);
            _textBoxView.continueHint.gameObject.SetActive(false);
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
            _textBoxView.normalText.SetText(string.Empty);
            _textBoxView.normalText.maxVisibleCharacters = 0;
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
            _textBoxView.normalText.text += text;

            int processedCharacters = 0;
            int totalCharacters = text.Length;
            
            while (processedCharacters < totalCharacters)
            {
                // If the player wants to continue, we immediately show the rest of
                // the text (the fast-reader button).
                if (_continueRequested)
                {
                    _textBoxView.normalText.maxVisibleCharacters += totalCharacters - processedCharacters;
                    RuntimeManager.PlayOneShot(_revealStyle.audioPerCharacter);
                    break;
                }

                // Skip past all of the style tags - e.g. <color=red>
                if (text[processedCharacters] == '<')
                    processedCharacters = text.IndexOf('>', processedCharacters) + 1;
                
                // We only want to chirp on visible characters, e.g. anything BUT a space
                if (text[processedCharacters] != ' ')
                    RuntimeManager.PlayOneShot(_revealStyle.audioPerCharacter);
                
                processedCharacters++;
                _textBoxView.normalText.maxVisibleCharacters++;
                await UniTask.Delay(_revealStyle.revealIntervalMs);
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
            
            public OptionBuilder(TextBoxPresenter presenter)
            {
                _presenter = presenter;
            }
            
            public async UniTask<int> Present()
            {
                EventSystem e = EventSystem.current;
                
                _presenter._textBoxView.optionParent.SetActive(true);
                e.SetSelectedGameObject(_views[0].button.gameObject);
                
                _presenter._textBoxView.normalFrame.gameObject.SetActive(false);
                _presenter._textBoxView.optionFrame.gameObject.SetActive(true);
                
                // Wait until an option is selected.
                while (_selectedOption == -1)
                {
                    Transform s = e.currentSelectedGameObject.transform;
                    Vector3 pos = s.position;
                    pos.x += ((RectTransform)s).rect.xMax; // align with the right edge of text
                    _presenter._textBoxView.optionSelectionHint.transform.position = pos;
                    await UniTask.Yield();
                }

                foreach (OptionView view in _views)
                    view.enabled = false;
                
                RuntimeManager.PlayOneShot(_presenter.optionSelectChirp);
                _presenter._textBoxView.normalFrame.gameObject.SetActive(true);
                _presenter._textBoxView.optionFrame.gameObject.SetActive(false);
                _presenter._textBoxView.optionParent.SetActive(false);
                _presenter._textBoxView.optionSelectionHint.gameObject.SetActive(false);
                
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
                view.textDisplay.autoSizeTextContainer = true;
                view.onSelect.AddListener(() => _selectedOption = index);
                view.onHover.AddListener(() => RuntimeManager.PlayOneShot(_presenter.optionHoverChirp));
                _views.Add(view);
                return this;
            }
        }
    }
}

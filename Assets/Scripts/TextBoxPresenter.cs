using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Ltg8
{
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [SerializeField] 
        private CanvasGroup textBoxGraphics;
        
        [SerializeField] 
        private TMP_Text textBoxText;
        
        [SerializeField] 
        private GameObject continueHint;

        [SerializeField]
        private OptionView optionPrefab;

        [SerializeField] 
        private CanvasGroup optionBoxGraphics;

        [SerializeField] 
        private CanvasGroup nameBoxGraphics;
        
        [SerializeField] 
        private TMP_Text nameBoxText;
        
        [SerializeField] 
        private float optionOpenDuration;
        
        [SerializeField] 
        private float optionCloseDuration;
        
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
        
        private bool _continueRequested;
        private ObjectPool<OptionView> _optionPool;

        private void Start()
        {
            _optionPool = new ObjectPool<OptionView>(() => {
                OptionView instance = Instantiate(optionPrefab, optionBoxGraphics.transform);
                instance.gameObject.SetActive(false);
                return instance;
            });
            continueHint.SetActive(false);
            nameBoxGraphics.gameObject.SetActive(false);
            optionBoxGraphics.gameObject.SetActive(false);
            textBoxText.text = string.Empty;
            textBoxText.maxVisibleCharacters = 0;
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
        }
        
        public void HandleContinue()
        {
            // Called by some UnityEvent or outside script
            // to signal the player is ready to read more text.
            _continueRequested = true;
        }

        public async UniTask ClearText()
        {
            textBoxText.text = string.Empty;
            textBoxText.maxVisibleCharacters = 0;
            await UniTask.Delay(TimeSpan.FromSeconds(clearDuration));
        }
        
        public async UniTask WriteText(string text)
        {
            _continueRequested = false;
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
            
            public OptionBuilder(TextBoxPresenter presenter)
            {
                _presenter = presenter;
            }
            
            public async UniTask<int> Present()
            {
                _presenter.optionBoxGraphics.gameObject.SetActive(true);
                await _presenter.FadeCanvasGroup(_presenter.optionBoxGraphics, 0, 1, _presenter.optionOpenDuration);

                // Wait until an option is selected.
                while (_selectedOption == -1)
                    await UniTask.Yield();
                
                RuntimeManager.PlayOneShot(_presenter.optionSelectChirp);

                await _presenter.FadeCanvasGroup(_presenter.optionBoxGraphics, 1, 0, _presenter.optionCloseDuration);
                _presenter.optionBoxGraphics.gameObject.SetActive(false);
                
                // Release all the borrowed options
                foreach (OptionView view in _views)
                {
                    view.gameObject.SetActive(false);
                    view.button.onClick.RemoveAllListeners();
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
                view.textDisplay.text = text;
                view.button.onClick.AddListener(() => _selectedOption = index);
                _views.Add(view);
                return this;
            }
        }
    }
}

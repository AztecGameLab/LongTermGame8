using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

// todo: more randomization on the chirps (ref celeste, talk w/ luke)
// todo: radial option box cleanup (needs audio, more control over different choice amounts - diff. view for each option amount?)
// todo: better animation support
// todo: better test w/ fake convo
// todo: smell w/ the option functions here
// todo: smell w/ duplicated logic between textbox and optionbox

namespace Ltg8
{
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [Title("Styles")]
        
        [SerializeField]
        private TextBoxView defaultTextBoxView;
        
        [SerializeField]
        private OptionBoxView defaultOptionBoxView;
        
        [SerializeField] 
        private RevealStyle defaultRevealStyle;
        
        [Title("Audio")]
        
        [SerializeField] 
        private EventReference confirmChirp;

        [Title("Settings")]

        [SerializeField]
        private float clearDuration;
        
        private IFlipBookAnimation _mainAnimation;
        private IFlipBookAnimation _optionAnimation;
        private bool _continueRequested;
        private RevealStyle _revealStyle;
        private TextBoxView _textBoxView;
        private OptionBoxView _optionBoxView;

        public void SetRevealStyle(RevealStyle style)
        {
            _revealStyle = style;
        }
        
        public void DefaultRevealStyle()
        {
            SetRevealStyle(defaultRevealStyle);
        }

        private void Start()
        {
            _textBoxView = defaultTextBoxView;
            _optionBoxView = defaultOptionBoxView;
            _revealStyle = defaultRevealStyle;
            
            _textBoxView.Initialize();
            _optionBoxView.Initialize();
        }

        private void Update()
        {
            if (_mainAnimation != null)
            {
                _mainAnimation.ApplyTo(_textBoxView.mainAnimationImage);
                _mainAnimation.ApplyTo(_optionBoxView.mainAnimationImage);
            }

            if (_optionAnimation != null)
                _optionAnimation.ApplyTo(_optionBoxView.optionAnimationImage);
        }

        public UniTask ShowMainAnimation(IFlipBookAnimation anim)
        {
            _mainAnimation = anim;
            _textBoxView.mainAnimationObject.SetActive(true);
            _optionBoxView.mainAnimationObject.SetActive(true);
            _mainAnimation.ApplyTo(_textBoxView.mainAnimationImage);
            _mainAnimation.ApplyTo(_optionBoxView.mainAnimationImage);
            return UniTask.CompletedTask;
        }

        public UniTask HideMainAnimation()
        {
            _mainAnimation = null;
            _textBoxView.mainAnimationObject.SetActive(false);
            _optionBoxView.mainAnimationObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public UniTask ShowOptionAnimation(IFlipBookAnimation anim)
        {
            _optionAnimation = anim;
            _optionBoxView.optionAnimationObject.SetActive(true);
            _optionAnimation.ApplyTo(_optionBoxView.optionAnimationImage);
            return UniTask.CompletedTask;
        }

        public UniTask HideOptionAnimation()
        {
            _optionAnimation = null;
            _optionBoxView.optionAnimationObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public UniTask Open()
        {
            _textBoxView.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
        
        public UniTask Close()
        {
            _textBoxView.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
        
        public async UniTask Open(string displayName)
        {
            SetName(displayName);
            ShowName();
            await Open();
        }

        public void ShowName()
        {
            _textBoxView.nameObject.SetActive(true);
        }

        public void HideName()
        {
            _textBoxView.nameObject.SetActive(true);
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
            _optionBoxView.mainText.SetText(string.Empty);
            _textBoxView.mainText.SetText(string.Empty);
            _textBoxView.mainText.maxVisibleCharacters = 0;
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
            _textBoxView.mainText.text += text;
            _optionBoxView.mainText.text += text;

            int processedCharacters = 0;
            int totalCharacters = text.Length;
            
            while (processedCharacters < totalCharacters)
            {
                // If the player wants to continue, we immediately show the rest of
                // the text (the fast-reader button).
                if (_continueRequested)
                {
                    _textBoxView.mainText.maxVisibleCharacters += totalCharacters - processedCharacters;
                    _optionBoxView.mainText.maxVisibleCharacters += totalCharacters - processedCharacters;
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
                _textBoxView.mainText.maxVisibleCharacters++;
                _optionBoxView.mainText.maxVisibleCharacters++;
                await UniTask.Delay(_revealStyle.revealIntervalMs);
            }
        }
        
        public async UniTask<int> PickOption(string first, string second)
        {
            _textBoxView.gameObject.SetActive(false);
            _optionBoxView.gameObject.SetActive(true);
            
            int result = await _optionBoxView.OptionPickTwo(first, second);
            
            _textBoxView.gameObject.SetActive(true);
            _optionBoxView.gameObject.SetActive(false);
            await ClearText();
            return result;
        }
        
        public async UniTask<int> PickOption(string first, string second, string third)
        {
            _textBoxView.gameObject.SetActive(false);
            _optionBoxView.gameObject.SetActive(true);
            
            int result = await _optionBoxView.OptionPickThree(first, second, third);
            
            _textBoxView.gameObject.SetActive(true);
            _optionBoxView.gameObject.SetActive(false);
            await ClearText();
            return result;
        }
        
        public async UniTask<int> PickOption(string first, string second, string third, string fourth)
        {
            _textBoxView.gameObject.SetActive(false);
            _optionBoxView.gameObject.SetActive(true);
            
            int result = await _optionBoxView.OptionPickFour(first, second, third, fourth);
            
            _textBoxView.gameObject.SetActive(true);
            _optionBoxView.gameObject.SetActive(false);
            await ClearText();
            return result;
        }
    }
}

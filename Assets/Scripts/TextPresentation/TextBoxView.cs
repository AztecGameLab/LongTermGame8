using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class TextBoxView : MonoBehaviour
    {
        [SerializeField] private TMP_Text mainText;
        [SerializeField] private GameObject mainAnimationObject;
        [SerializeField] private RawImageFlipBookView mainAnimationImage;
        [SerializeField] private GameObject continueHint;
        [SerializeField] private EventReference confirmChirp;
        [SerializeField] private RevealStyle defaultRevealStyle;
        [SerializeField] private float clearDuration;
        [SerializeField] private GameObject nameObject;
        [SerializeField] private TMP_Text nameBoxText;
        
        private bool _continueRequested;
        private string _currentDisplayName;
        private IFlipBookAnimation _currentMainAnimation;

        public void ResetAllState()
        {
            CurrentText = string.Empty;
            CurrentDisplayName = string.Empty;
            CurrentMainAnimation = null;
            CurrentRevealStyle = defaultRevealStyle;
        }

        private void OnEnable()
        {
            Ltg8.Controls.GameplayCommon.Confirm.performed += HandleConfirm;
        }

        private void OnDisable()
        {
            Ltg8.Controls.GameplayCommon.Confirm.performed -= HandleConfirm;
        }
        
        private void HandleConfirm(InputAction.CallbackContext context)
        {
            _continueRequested = true;
        }

        private void Update()
        {
            CurrentMainAnimation?.ApplyTo(mainAnimationImage);
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

        public async UniTask WriteText(string text)
        {
            mainText.text += text;

            int processedCharacters = 0;
            int totalCharacters = text.Length;
            
            while (processedCharacters < totalCharacters)
            {
                // If the player wants to continue, we immediately show the rest of
                // the text (the fast-reader button).
                if (_continueRequested)
                {
                    mainText.maxVisibleCharacters += totalCharacters - processedCharacters;
                    RuntimeManager.PlayOneShot(CurrentRevealStyle.audioPerCharacter);
                    break;
                }

                // Skip past all of the style tags - e.g. <color=red>
                if (text[processedCharacters] == '<')
                    processedCharacters = text.IndexOf('>', processedCharacters) + 1;
                
                // We only want to chirp on visible characters, e.g. anything BUT a space
                if (text[processedCharacters] != ' ')
                    RuntimeManager.PlayOneShot(CurrentRevealStyle.audioPerCharacter);
                
                processedCharacters++;
                mainText.maxVisibleCharacters++;
                await UniTask.Delay(CurrentRevealStyle.revealIntervalMs);
            }
        } 
        
        public async UniTask ClearText()
        {
            CurrentText = string.Empty;
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

        public string CurrentText
        {
            get => mainText.text;
            set
            {
                mainText.SetText(value);
                mainText.maxVisibleCharacters = value.Length;
            }
        }

        public RevealStyle CurrentRevealStyle { get; set; }

        public IFlipBookAnimation CurrentMainAnimation
        {
            get => _currentMainAnimation;
            set
            {
                mainAnimationObject.SetActive(value != null);
                value?.ApplyTo(mainAnimationImage);
                _currentMainAnimation = value;
            }
        }
        
        public string CurrentDisplayName
        {
            get => _currentDisplayName;
            set
            {
                nameObject.SetActive(value != string.Empty);
                nameBoxText.SetText(value);
                _currentDisplayName = value;
            }
        }
    }
}

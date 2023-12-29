using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;

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
        
        private bool _continueRequested;

        private void Start()
        {
            textBoxText.text = string.Empty;
            textBoxText.maxVisibleCharacters = 0;
        }

        public async UniTask Open()
        {
            // Simple fade-in animation
            float elapsed = 0;
            
            while (elapsed <= openDuration)
            {
                elapsed += Time.deltaTime;
                textBoxGraphics.alpha = Mathf.Lerp(0, 1, elapsed / openDuration);
                await UniTask.Yield();
            }

            textBoxGraphics.alpha = 1;
        }
        
        public async UniTask Close()
        {
            // Simple fade-out animation
            float elapsed = 0;
            
            while (elapsed <= closeDuration)
            {
                elapsed += Time.deltaTime;
                textBoxGraphics.alpha = Mathf.Lerp(1, 0, elapsed / closeDuration);
                await UniTask.Yield();
            }

            textBoxGraphics.alpha = 0;
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
    }
}

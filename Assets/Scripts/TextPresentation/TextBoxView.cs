using System;
using Animation.FlipBook;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Plugins.FMOD.src;
using PluginScripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TextPresentation
{
    public class TextBoxView : MonoBehaviour
    {
        [SerializeField] private TMP_Text mainText; // Main Dialogue Box Text
        [SerializeField] private GameObject mainAnimationObject; // Empty GameObject for ImageFlipBook
        [SerializeField] private ImageFlipbookView mainAnimationImage; // The image of the one talking
        [SerializeField] private GameObject continueHint; // Icon that appears when you can continue text
        [SerializeField] private EventReference confirmChirp; 
        [SerializeField] private RevealStyle defaultRevealStyle; 
        [SerializeField] private float clearDuration; // The amount of time to wait after clearing text
        [SerializeField] private GameObject nameObject; // Empty GameObject for character name
        [SerializeField] private TMP_Text nameBoxText; // Character name
        [SerializeField] private SpriteFlipBookAnimation defaultAnimation; // Animation for the character talking
        
        private bool _continueRequested; // Determines whether the player has chosen to "Continue"
        private string _currentDisplayName; // The name for the character currently talking
        private IFlipBookAnimation _currentMainAnimation; // The animation currently playing

        // Resets all the details of the dialogue box to their default values
        public void ResetAllState()
        {
            CurrentText = string.Empty; // Sets the current text to nothing
            CurrentDisplayName = string.Empty; // Sets the current "speaker" name to nothing
            CurrentMainAnimation = defaultAnimation; // Sets the current animation to a default animation
            CurrentRevealStyle = defaultRevealStyle; // Sets the RevealStyle to the default reveal style
        }
        
        private void OnEnable() // NOTE: Come back to this, more inspection is needed
        {
            Ltg8.Ltg8.Controls.GameplayCommon.Confirm.performed += HandleConfirm;
        }

        private void OnDisable() // NOTE: Come back to this, more inspection is needed
        {
            Ltg8.Ltg8.Controls.GameplayCommon.Confirm.performed -= HandleConfirm;
        }

        // Determines if the console is open
        private static bool IsConsoleOpen()
        {
            var console = GameObject.Find("Runtime Console").GetComponent<RuntimeConsole>();
            return console.IsVisible();
        }
        
        private void HandleConfirm(InputAction.CallbackContext context) // NOTE: Come back to this, more inspection is needed
        {
            if (IsConsoleOpen()) return;
            _continueRequested = true;
        }
        
        private void Update()
        {
            defaultAnimation.Update(Time.deltaTime); // Updates the time elapsed for the defaultAnimation
            /*
               NOTE: | The ?. here is a null-conditional operator. The code will only execute if the |
               NOTE: | CurrentMainAnimation value is not null                                        |
            */ 
            CurrentMainAnimation?.ApplyTo(mainAnimationImage); // NOTE: Come back to this, more inspection is needed
        }

        // NOTE: Async is used for the "Await" command, allowing delays or waiting for other executions
        // NOTE: It seems that UniTask is a plugin, and is not automatically included in Unity
        public async UniTask WaitForContinue()
        {
            _continueRequested = false; // Player has not requested to continue
            continueHint.gameObject.SetActive(true); // Show the icon that indicates that contiuing is possible

            // Will not finish until `HandleContinue` is called.
            while (!_continueRequested) // While the player has not requested to continue
                await UniTask.Yield(); // Wait

            RuntimeManager.PlayOneShot(confirmChirp); // NOTE: Come back to this, more inspection is needed
            continueHint.gameObject.SetActive(false); // Hide the "Continue" icon
            _continueRequested = false; // Reset value
        }

        /* This method adds the text specified in the parameter, and then reveals the characters
         one by one to create the text-typing efect */
        public async UniTask WriteText(string text)
        {
            mainText.text += text; // Add text to the main dialogue text

            var processedCharacters = 0; // As text is shown, this keeps track of the position in the text
            var totalCharacters = text.Length; // Total amount of characters in the added text
            
            while (processedCharacters < totalCharacters) // While not all characters have been processed
            {
                // If the player wants to continue, we immediately show the rest of
                // the text (the fast-reader button).
                if (_continueRequested) 
                {
                    // Show the remaining characters
                    mainText.maxVisibleCharacters += totalCharacters - processedCharacters;
                    // Removed Command -> RuntimeManager.PlayOneShot(CurrentRevealStyle.audioPerCharacter);
                    break;
                }

                // Skip past all of the style tags - e.g. <color=red>
                if (text[processedCharacters] == '<')
                    processedCharacters = text.IndexOf('>', processedCharacters) + 1;
                
                // We only want to chirp on visible characters, e.g. anything BUT a space
                // More Removed Code -> if (processedCharacters < text.Length && text[processedCharacters] != ' ')
                                            // RuntimeManager.PlayOneShot(CurrentRevealStyle.audioPerCharacter);
                
                processedCharacters++; // Increments position tracker
                mainText.maxVisibleCharacters++; // Shows the next letter in the dialogue
                await UniTask.Delay(CurrentRevealStyle.revealIntervalMs); // Waits (increment based on style)
            }
        } 
        
        // Erases all the text from the dialogue box
        public async UniTask ClearText()
        {
            CurrentText = string.Empty; // Removes all text
            await UniTask.Delay(TimeSpan.FromSeconds(clearDuration)); // Waits the specified amount of time 
        }

        // Stops the code's execution for the time (in seconds) specified in the parameters
        public async UniTask Delay(float seconds)
        {
            float elapsed = 0; // Time elapsed starts at 0, no time has passed

            /* While the time elapsed is less than the total time to wait AND the player has not
             requested to continue */
            while (elapsed < seconds && !_continueRequested) 
            {
                elapsed += Time.deltaTime; // Increment time
                await UniTask.Yield(); // Yield
            }
        }

        // The current text presented in the dialogue box, Getters and Setters
        // NOTE: Come back to this, more inspection is needed
        public string CurrentText
        {
            get => mainText.text; // Gets "main text" text
            set
            {
                mainText.SetText(value); // Main text set to text of the current CurrentText value
                mainText.maxVisibleCharacters = value.Length; /* Amount of mainText characters shown set
                to the length of the CurrentText string */
            }
        }

        public RevealStyle CurrentRevealStyle { get; set; } // The CurrentRevealStyle getters/setters

        public IFlipBookAnimation CurrentMainAnimation // The CurrentMainAnimation getters/setters
        {
            get => _currentMainAnimation; // Get the current main animation from this script
            set
            {
                mainAnimationObject.SetActive(value != null); /* Set the mainAnimationObject to active 
                if the CurrentMainAnimation is not null*/
                value?.ApplyTo(mainAnimationImage); // If CurrentMainAnimation is not null, ____
                _currentMainAnimation = value; // This script's currentMainAnimation is set to CurrentMainAnimation
            }
        }
        
        public string CurrentDisplayName // CurrentDisplayName getters/setters
        {
            get => _currentDisplayName; // Return this scripts currentDisplayName
            set
            {
                nameObject.SetActive(value != string.Empty); /* Empty object for the name is set to visible
                if the string is not empty*/
                nameBoxText.SetText(value); // Set the nameBox's text to the CurrentDisplayName value
                _currentDisplayName = value; // Set this script's currentDisplayName to CurrentDisplayName
            }
        }
    }
}

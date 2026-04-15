using System;
using Animation.FlipBook;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Plugins.FMOD.src;
using PluginScripts;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TextPresentation
{
    /// <summary>
    ///   <b>Description</b>: This class is responsible for the dialogue text box,
    /// performing tasks such as clearing, writing, and revealing text dialogue. 
    /// </summary>
    [HideMonoScript]
    public class TextBoxView : MonoBehaviour
    {
        
        [Title("Dialogue Box")]
        
        [Header("UI Elements")]
        
        [Tooltip("Text for the dialogue")]
        [SerializeField] private TMP_Text mainText; 
        [Tooltip("UI GameObject [w/ Image] for the character speaking")]
        [SerializeField] private GameObject mainAnimationObject; 
        [Tooltip("UI GameObject [w/ Image] for the icon that says you can continue")]
        [SerializeField] private GameObject continueHint;
        [Tooltip("UI GameObject that holds the character's name and dialogue frame")]
        [SerializeField] private GameObject nameObject; 
        [Tooltip("Text for the character's name")]
        [SerializeField] private TMP_Text nameBoxText; 
        
        [Header("Settings")]
        
        [SerializeField] private EventReference confirmChirp; // Audio???
        [Tooltip("Default style for how text is revealed")]
        [SerializeField] private RevealStyle defaultRevealStyle; 
        [Tooltip("The amount of time to wait after clearing text")]
        [SerializeField] private float clearDuration; 
        
        [Header("Sprite Animation")]
        
        [Tooltip("Script for changing the dialogue sprites")]
        [SerializeField] private ImageFlipbookView mainAnimationImage;
        [Tooltip("Script for controlling sprite animations")]
        [SerializeField] private SpriteFlipBookAnimation defaultAnimation; 
        
        /// <summary> <b>Description</b>: Boolean to determines whether the player has chosen to "Continue" </summary>
        private bool _continueRequested; 
        /// <summary> <b>Description</b>: The name for the character currently talking </summary>
        private string _currentDisplayName; 
        /// <summary> <b>Description</b>: Holds the current sprite animation controller </summary>
        private IFlipBookAnimation _currentMainAnimation; 

        /// <summary> <b>Description</b>: Resets all the components of the dialogue box to default values </summary>
        public void ResetAllState()
        {
            CurrentText = string.Empty; // Sets the current dialogue text to nothing
            CurrentDisplayName = string.Empty; // Sets the current "speaker" name to nothing
            CurrentMainAnimation = defaultAnimation; // Sets the current animation (data) to default 
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

        /// <summary> <b>Description</b>: Determines if the game console is open </summary>
        private static bool IsConsoleOpen()
        {
            var console = GameObject.Find("Runtime Console").GetComponent<RuntimeConsole>();
            return console.IsVisible();
        }
        
        /// <summary>
        /// Once the player has confirmed to continue, set _continueRequested to true, unless
        /// the console is open
        /// </summary>
        /// <param name="context">The input that triggered the event that called this method</param>
        private void HandleConfirm(InputAction.CallbackContext context) 
        {
            if (IsConsoleOpen()) return; // If console is open, the player cannot continue dialogue
            _continueRequested = true; // The player has requested to continue
        }
        
        // Runs every frame, causing the counter for the Animation Controller to update
        private void Update()
        {
            defaultAnimation.Update(Time.deltaTime); // Updates the time elapsed for the defaultAnimation
            
            CurrentMainAnimation?.ApplyTo(mainAnimationImage); // If there is an animation controller, transition the Flipbook to the next frame
        }

        /// <summary>
        ///   <b>Description</b>: Holds the dialogue execution until the player chooses to continue.
        /// </summary>
        public async UniTask WaitForContinue()
        {
            _continueRequested = false; // Player has not requested to continue
            continueHint.gameObject.SetActive(true); // Show the icon that indicates that contiuing is possible

            // Will not finish until `HandleContinue` is called.
            while (!_continueRequested) // While the player has not requested to continue
                await UniTask.Yield(); // Wait

            RuntimeManager.PlayOneShot(confirmChirp); // NOTE: Come back to this, more inspection is needed (Audio stuff presumably) 
            continueHint.gameObject.SetActive(false); // Hide the "Continue" icon
            _continueRequested = false; // Reset value
        }

        /// <summary>
        ///   <b>Description</b>: This method writes the game dialogue, revealings the characters
        ///     one by one to create the text-typing effect.
        /// </summary>
        /// <param name="text"> The dialogue text to display </param>
        public async UniTask WriteText(string text)
        {
            mainText.text += text; // Add text to the main dialogue text

            var processedCharacters = 0; // As text is shown, this keeps track of the position in the text
            var totalCharacters = text.Length;
            
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

                // Skip past all the style tags - e.g. <color=red>
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
        
        /// <summary>
        ///   <b>Description</b>: Erases all the text from the dialogue box
        /// </summary>
        public async UniTask ClearText()
        {
            CurrentText = string.Empty; // Removes all text
            await UniTask.Delay(TimeSpan.FromSeconds(clearDuration)); // Waits the specified amount of time 
        }
        
        /// <summary>
        ///   <b>Description</b>: Stops the dialogue's execution until the player has chosen to continue AND a specified amount of time has passed
        /// </summary>
        /// <param name="seconds"> Time to delay the dialogue's execution </param>
        public async UniTask Delay(float seconds)
        {
            float elapsed = 0; // Time elapsed starts at 0, no time has passed

            /* While the time elapsed is less than the total time to wait AND the player has not
             requested to continue */
            while (elapsed < seconds && !_continueRequested) 
            {
                elapsed += Time.deltaTime; // Increment time
                await UniTask.Yield(); 
            }
        }

        /// <summary> <b>Description</b>: The current text presented in the dialogue box, Getters and Setters </summary>
        public string CurrentText
        {
            get => mainText.text; // Gets "main text" text
            set
            {
                mainText.SetText(value); // Dialogue text set to the "set" value given
                mainText.maxVisibleCharacters = value.Length; /* Amount of mainText characters shown set
                to the length of the CurrentText string */
            }
        }

        /// <summary> <b>Description</b>: The dialogue's reveal style, Getters and Setters </summary>
        public RevealStyle CurrentRevealStyle { get; set; } 

        /// <summary> <b>Description</b>: The Flipbook animation controller, Getters and Setters </summary>
        public IFlipBookAnimation CurrentMainAnimation 
        {
            get => _currentMainAnimation; // Get the current flipbook animation controller
            set
            {
                mainAnimationObject.SetActive(value != null); /* Set the flipBook object to active (if the set-value is not null)*/
                value?.ApplyTo(mainAnimationImage); // If set-value is not null, display next Flipbook frame in sequence
                _currentMainAnimation = value; // This script's currentMainAnimation is set to this set-value
            }
        }
        
        /// <summary> <b>Description</b>: Current speaker name, getters / setters </summary>
        public string CurrentDisplayName 
        {
            get => _currentDisplayName; // Return the name of the current speaker 
            set
            {
                nameObject.SetActive(value != string.Empty); // Set name-text object to active if the set-value is not null
                nameBoxText.SetText(value); // Set the nameBox's text to the set-value
                _currentDisplayName = value; // Set this script's currentDisplayName to the set-value
            }
        }
    }
}

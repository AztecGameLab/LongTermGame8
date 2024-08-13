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
        /// <summary> <b>Description</b>: [Text] for the dialogue </summary>
        [SerializeField] private TMP_Text mainText; 
        /// <summary> <b>Description</b>: GameObject [w/ Image] for the character speaking </summary>
        [SerializeField] private GameObject mainAnimationObject; 
        /// <summary> <b>Description</b>: [Script] for the FlipBook image display </summary>
        [SerializeField] private ImageFlipbookView mainAnimationImage;
        /// <summary> <b>Description</b>: GameObject [w/ Image] for the icon that says you can continue </summary>
        [SerializeField] private GameObject continueHint; 
        [SerializeField] private EventReference confirmChirp; // Audio???
        /// <summary> <b>Description</b>: [Scriptable Object] default style for how text is revealed </summary>
        [SerializeField] private RevealStyle defaultRevealStyle; 
        /// <summary> <b>Description</b>: [Float] The amount of time to wait before clearing text </summary>
        [SerializeField] private float clearDuration; 
        /// <summary> <b>Description</b>: [Empty GameObject] holds character's name and dialogue frame </summary>
        [SerializeField] private GameObject nameObject; 
        /// <summary> <b>Description</b>: [Text] Text for the character's name </summary>
        [SerializeField] private TMP_Text nameBoxText; 
        /// <summary> <b>Description</b>: [Serializable Class] Controller for the sprite animation </summary>
        [SerializeField] private SpriteFlipBookAnimation defaultAnimation; 
        
        /// <summary> <b>Description</b>: [Boolean] Determines whether the player has chosen to "Continue" </summary>
        private bool _continueRequested; 
        /// <summary> <b>Description</b>: [String] The name for the character currently talking </summary>
        private string _currentDisplayName; 
        /// <summary> <b>Description</b>: [Implementation of Interface] Holds the current sprite animation controller </summary>
        private IFlipBookAnimation _currentMainAnimation; 

        /// <summary>
        ///   <b>Description</b>: Resets all the components of the dialogue box to their default values
        /// </summary>
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

        /// <summary>
        ///   <b>Description</b>: Determines if the game console is open
        /// </summary>
        private static bool IsConsoleOpen()
        {
            var console = GameObject.Find("Runtime Console").GetComponent<RuntimeConsole>();
            return console.IsVisible();
        }
        
        private void HandleConfirm(InputAction.CallbackContext context) // NOTE: The "context" is what is returned by this method (despite it being Void)
        {
            if (IsConsoleOpen()) return;
            _continueRequested = true; // The player has requested to continue
        }
        
        // Updates the counter, and updates the Flipbook at the intervals that have been defined and that are managed by the counter
        private void Update()
        {
            defaultAnimation.Update(Time.deltaTime); // Updates the time elapsed for the defaultAnimation
            
            // NOTE: The ?. here is a null-conditional operator. The code will only execute if the CurrentMainAnimation value is not null                                        |
            
            CurrentMainAnimation?.ApplyTo(mainAnimationImage); // If there is an animation controller, transition the Flipbook to the next frame
        }

        // NOTE: Async is used for the "Await" command, allowing delays or waiting for other executions
        // NOTE: It seems that UniTask is a plugin, and is not automatically included in Unity
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
                await UniTask.Yield(); // Yield
            }
        }

        /// <summary> <b>Description</b>: The current text presented in the dialogue box, Getters and Setters </summary>
        public string CurrentText
        {
            get => mainText.text; // Gets "main text" text
            set
            {
                mainText.SetText(value); // Dialogue text set to the value of CurrentText
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
                mainAnimationObject.SetActive(value != null); /* Set the flipBook object to active (if the CurrentMainAnimation is not null)*/
                value?.ApplyTo(mainAnimationImage); // If CurrentMainAnimation is not null, display next Flipbook frame in sequence
                _currentMainAnimation = value; // This script's currentMainAnimation is set to CurrentMainAnimation
            }
        }
        
        /// <summary> <b>Description</b>: Current speaker name, getters / setters </summary>
        public string CurrentDisplayName 
        {
            get => _currentDisplayName; // Return the name of the current speaker 
            set
            {
                nameObject.SetActive(value != string.Empty); // Set name-text object to active if the current name is not null
                nameBoxText.SetText(value); // Set the nameBox's text to the CurrentDisplayName value
                _currentDisplayName = value; // Set this script's currentDisplayName to CurrentDisplayName
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using poetools.Console;
using poetools.Console.Commands;
using poetools.PluginScripts.Executions;
using UnityEngine;
using UnityEngine.UI;

namespace PluginScripts
{
    /// <summary>
    ///   <b>Description</b>: Script responsible for the functionality of the "admin" console in-game, such as
    /// <list type="number">
    ///       <item>
    ///           <description><b>Commands</b> - The initialization and execution of commands </description>
    ///       </item>
    ///       <item>
    ///           <description><b>User Input</b> - Auto-completion, input history, etc</description>
    ///       </item>
    ///       <item>
    ///           <description><b>Output</b> - Logs of user commands, console messages, and errors </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class RuntimeConsole : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("[Int] The number of commands that should be remembered after running.")]
        private int maxCommandHistory = 30;

        [SerializeField]
        [Tooltip("[Bool] Whether the console should begin opened or closed.")]
        private bool startVisible;
        
        [SerializeField]
        [Tooltip("[Array] The commands that are registered with the console by default.")]
        private Command[] autoRegisterCommands;

        [Header("Style")]

        [SerializeField]
        [Tooltip("[LogPrefix] The prefix that is shown before logged messages.")]
        private LogPrefix logPrefix;

        [SerializeField]
        [Tooltip("[UserPrefix] The prefix that is shown before used-entered messages.")]
        private UserPrefix userPrefix;

        [Header("References")]

        [SerializeField]
        [Tooltip("The text that displays the main console output.")]
        private Text textDisplay;

        [SerializeField]
        [Tooltip("The text that displays auto-complete options.")]
        private Text autoCompleteDisplay;

        [SerializeField]
        [Tooltip("The input field where the user enters console commands.")]
        private InputField inputFieldDisplay;

        [SerializeField]
        [Tooltip("The GameObject that holds the console view components")]
        private GameObject consoleView;
        
        /// <summary> <b>Description</b>: List of all game command instances </summary>
        private List<ICommand> _commandInstances;
        
        // Note: Come back to this
        /// <summary> <b>Description</b>: The suggested commands for the console </summary>
        private List<string> _suggestions; 
        /// <summary> <b>Description</b>: The index for the currently selected suggestion </summary>
        private int _autoCompleteIndex; 
        
        private string _oldValue;

        /// <summary> <b>Description</b>: An event that is triggered upon the creation of the Runtime Console </summary>
        public static event Action<CreateEvent> OnCreate; 
        /// <summary> <b>Description</b>: An event that is triggered once the visibility of the Console is modified </summary>
        public event Action<bool, bool> OnVisibilityChanged; 

        /// <summary> <b>Description</b>: The Command "Manager" for this Console </summary>
        public CommandRegistry CommandRegistry { get; private set; }
        
        /// <summary> <b>Description</b>: The Command History for this console </summary>
        private IInputHistory InputHistory { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] // TODO: Learn this later
        
        private static void Init() // Initialization for OnCreate, I guess. Doesn't seem to have a purpose.
        {
            OnCreate = null;
        }

        // Functions to run upon the creation of the runtime console
        private void Awake() 
        {
            // Iniitalize the Command Registry, Input History, and commandInstances / suggestions lists 
            CommandRegistry = new CommandRegistry(); 
            InputHistory = new InputHistory(maxCommandHistory);
            _commandInstances = new List<ICommand>();
            _suggestions = new List<string>();

            // Initialize the the console's visibility.
            SetVisible(startVisible);

            // Register default commands.
            foreach (Command command in autoRegisterCommands)
            {
                Command instance = Instantiate(command); // Create an instance of the command
                instance.Initialize(this); // Initialize it (if needed)
                CommandRegistry.Register(instance); // Add it to the command registry
                _commandInstances.Add(instance); // Add it to the list of all command instances
            }

            /* Once the console has been created, the OnCreate event is triggered, w/ a new CreateEvent structure
             with this console. */
            OnCreate?.Invoke(new CreateEvent{Console = this}); 
        }

        // Functions to run upon the destruction of the Runtime Console object
        private void OnDestroy()
        {
            // Dispose of every command
            foreach (ICommand command in _commandInstances) 
                command.Dispose();

            // Dispose of the command registry
            CommandRegistry.Dispose();
        }

        // Functions to run once the Runtime Console GameObject is enabled 
        private void OnEnable()
        {
            // Subscribes HandleVisibilityChange to the OnVisibilityChange event
            OnVisibilityChanged += HandleVisibilityChange; 
            // Make it so the "HandleSubmit" function is called during an onSubmit event
            inputFieldDisplay.onSubmit.AddListener(HandleSubmit);
            // Make it so the "HandleInputChange" function is called during an onValueChanged event 
            inputFieldDisplay.onValueChanged.AddListener(HandleInputChange);
        }

        // Functions to run once the Runtime Console GameObject is disabled
        private void OnDisable()
        {
            // Unubscribes HandleVisibilityChange from the OnVisibilityChange event
            OnVisibilityChanged -= HandleVisibilityChange; 
            // Removes "HandleSubmit"s listener for the onSubmit event
            inputFieldDisplay.onSubmit.RemoveListener(HandleSubmit);
            // Removes "HandleInputChange"s listener for the onValueChanged event
            inputFieldDisplay.onValueChanged.RemoveListener(HandleInputChange);
        }

        // === Public API ===
        /// <summary>
        ///   <b>Description</b>: 
        /// </summary>
        /// <param name="category"> The category of the message, used for the prefix </param>
        /// <param name="message"> The message to log on the console </param>
        public void Log(string category, string message)
        {
            string header = logPrefix.GenerateMessage(category); // Creates a prefix for the Log message
            LogRaw($"{header}{message}"); // Logs the message with the created prefix
        }

        /// <summary>
        ///   <b>Description</b>: Adds the given message to the Runtime Console on a new line.
        /// </summary>
        /// <param name="message"> The message to log in the console </param>
        public void LogRaw(string message)
        {
            textDisplay.text += $"\n{message}"; 
        }

        /// <summary>
        ///   <b>Description</b>: Clears the console
        /// </summary>
        public void Clear()
        {
            textDisplay.text = string.Empty;
        }

        /// <summary>
        ///   <b>Description</b>: Executes the given command.
        /// </summary>
        /// <param name="console"> The command to run </param>
        public void Execute(string command)
        {
            HandleSubmit(command);
        }

        /// <summary>
        ///   <b>Description</b>: Toggles the Visibility of the Console
        /// </summary>
        public void ToggleVisibility()
        {
            SetVisible(!IsVisible()); // Toggles the console's visibility
            var player = GameObject.Find("Player"); // Finds the "Player" object
            if (IsVisible()) // If the console is now visible
            {
                player.GetComponent<CharacterController>().enabled = false; // Disables the CharacterController
                player.GetComponent<NoClipMovement>().SetConsole(true); // Stupid Justin Script, make better later - Justin
            }
            else if(player.GetComponent<NoClipMovement>().GetNoclip()) // If The console is not open, but noclip is enabled
            {
                player.GetComponent<CharacterController>().enabled = false; // Keep CharacterController disabled
                player.GetComponent<NoClipMovement>().SetConsole(false); // Can we ban this guy? - Justin
            }
            else
            {
                player.GetComponent<CharacterController>().enabled = true; // Re-eanble the CharacterController
            }
        }

        /// <summary>
        ///   <b>Description</b>: Sets the visibility of the console
        /// </summary>
        /// <param name="isVisible"> If true, the console is visible </param>
        public void SetVisible(bool isVisible)
        {
            bool wasVisible = consoleView.activeSelf; // Boolean, console's previous visibility
            consoleView.SetActive(isVisible); // Determines' console's visibility 
            OnVisibilityChanged?.Invoke(wasVisible, isVisible); // Invokes the OnVisiblityChanged event
        }

        /// <summary>
        ///   <b>Description</b>: Returns whether the console is visible
        /// </summary>
        public bool IsVisible()
        {
            return consoleView.activeSelf;
        }

        /// <summary>
        ///   <b>Description</b>: Cycles the suggested auto-completions forward one
        /// </summary>
        public void CycleAutoCompleteForward()
        {
            _autoCompleteIndex++; // Increments the index for the selected auto-completion
            UpdateAutoCompleteText(); // Updates auto-complete texts
        }

        /// <summary>
        ///   <b>Description</b>: Cycles the suggest auto-completes backwards one
        /// </summary>
        public void CycleAutoCompleteBackward()
        {
            _autoCompleteIndex--; // Decrements the index for the selected auto-completion
            UpdateAutoCompleteText(); // Updates auto-complete texts
        }

        /// <summary>
        ///   <b>Description</b>: Cycles the input-history backwards one
        /// </summary>
        public void MoveHistoryBackward()
        {
            // Try cycling the input history backwards one, outputting the previous command
            if (InputHistory.TryMoveBackwards(out string prevCommand)) 
                // Set the text in the inputField to the previous command
                inputFieldDisplay.text = prevCommand;
        }

        /// <summary>
        ///   <b>Description</b>: Cycles the input-history forwards one
        /// </summary>
        public void MoveHistoryForward()
        {
            // Try cycling the input history forwards one, outputting the next command
            if (InputHistory.TryMoveForwards(out string nextCommand))
                // Set the text in the inputField to the next command
                inputFieldDisplay.text = nextCommand;
        }

        // === Event Handlers ===
        
        /// <summary>
        ///   <b>Description</b>: The code to execute when a command is submitted in the console
        /// </summary>
        /// <param name="input"> The user-input from the console </param>
        private void HandleSubmit(string input)
        {
            // Separates the given string into substrings, with spaces acting as the separator 
            string[] splitInput = input.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            // If the input is not empty and the splitInput array is not empty
            if (input.Length > 0 && splitInput.Length > 0)
            {
                string[] args = ArgumentTools.Parse(splitInput); // Removes the command, leaving only an array of arguments
                ICommand command = CommandRegistry.FindCommand(splitInput[0]); // Returns the command given in "input"
                // Displays the user-inputted command in the console, with the userPrefix attached
                textDisplay.text += '\n' + userPrefix.GenerateMessage(input); 
                // Executes the command w/ its arguments
                command.Execute(args, this);
                // Adds the user's input to the Input History 
                InputHistory.AddEntry(input);
            }

            ResetInputField();
        }

        /// <summary>
        ///   <b>Description</b>: The code to execute when the console becomes visible
        /// </summary>
        /// <param name="isVisible"> If true, the console is visible </param>
        /// <param name="wasvisible"> If true, the console was visible before the change </param>
        private void HandleVisibilityChange(bool wasVisible, bool isVisible)
        {
            if (isVisible) // If the console is visible
                ResetInputField(); // Reset the state of the input field
        }

        /// <summary>
        ///   <b>Description</b>: The code to execute when the the input in the console is changed
        /// </summary>
        /// <param name="value"> The current console text </param>
        private void HandleInputChange(string value)
        {
            // Separates the input into substrings, using spaces as a separator 
            string[] splitInput = value.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            // If the above array is empty (no input)
            if (splitInput.Length == 0)
            {
                // case: we have no input.
                autoCompleteDisplay.text = string.Empty; // Make the autoComplete empty
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)) // If space is pressed
            {
                int newSpaceCount = value.Count(c => c == ' '); // Count the amount of spaces in "value"
                int oldSpaceCount = _oldValue.Count(c => c == ' '); // Count the amount of spaces in "old value"

                // If we entered a space, apply the auto-complete.
                if (_suggestions.Any() && newSpaceCount > oldSpaceCount)
                {
                    _autoCompleteIndex = 0; // Set the auto-complete back to the first suggestion
                    string autoCompleteText = autoCompleteDisplay.text; // Grab the auto-complete text
                    inputFieldDisplay.text = autoCompleteText + " "; // Set the input Field to the auto-complete text with a space
                    // Place the caret (the little ^ symbol that shows up when you type) to the end of the input field text
                    inputFieldDisplay.caretPosition = inputFieldDisplay.text.Length; 
                } 
            }
            // Fill the suggestions list with all possible auto-completions 
            CommandRegistry.FindCommands(inputFieldDisplay.text, _suggestions); 
            UpdateAutoCompleteText(); // Update the auto-complete text
            _oldValue = value; // Set the oldValue (for spaces) to the current value
        }

        // === Helpers ===
        /// <summary>
        ///   <b>Description</b>: Updates the auto-complete text shown when typing something into the console
        /// </summary>
        private void UpdateAutoCompleteText()
        {
            // If the index surpasses the amount of suggestions, loop it back to the first suggestion (index 0)
            int index = (int) Mathf.Repeat(_autoCompleteIndex, _suggestions.Count); 
            // If there are existing suggestions, set the autoCompleteText to the suggestion at the index
            // Otherwise, make it empty
            string autoCompleteText = _suggestions.Count > 0 ? _suggestions[index] : "";
            // Set the auto-complete text to the string above 
            autoCompleteDisplay.text = autoCompleteText;
        }

        /// <summary>
        ///   <b>Description</b>: Resets the state of the Console's Input Field
        /// </summary>
        private void ResetInputField()
        {
            inputFieldDisplay.text = string.Empty; // Clear the input field text
            inputFieldDisplay.ActivateInputField(); // Activate / Enable the input field 
            InputHistory.Clear(); // Clear the input history
        }

        // === Structures ===
        /// <summary> <b>Description</b>: Custom Structure, w/ a RuntimeConsole Object </summary>
        public struct CreateEvent 
        {
            public RuntimeConsole Console;
        }
    }
}

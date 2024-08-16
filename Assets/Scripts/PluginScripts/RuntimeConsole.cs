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
        private List<string> _suggestions; 
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
        public void Log(string category, string message)
        {
            string header = logPrefix.GenerateMessage(category);
            LogRaw($"{header}{message}");
        }

        public void LogRaw(string message)
        {
            textDisplay.text += $"\n{message}";
        }

        public void Clear()
        {
            textDisplay.text = string.Empty;
        }

        public void Execute(string command)
        {
            HandleSubmit(command);
        }

        public void ToggleVisibility()
        {
            SetVisible(!IsVisible());
            var player = GameObject.Find("Player");
            if (IsVisible())
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<NoClipMovement>().SetConsole(true);
            }
            else if(player.GetComponent<NoClipMovement>().GetNoclip())
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<NoClipMovement>().SetConsole(false);
            }
            else
            {
                player.GetComponent<CharacterController>().enabled = true;
            }
        }

        public void SetVisible(bool isVisible)
        {
            bool wasVisible = consoleView.activeSelf; // Boolean, console's previous visibility
            consoleView.SetActive(isVisible); // Determines' console's visibility 
            OnVisibilityChanged?.Invoke(wasVisible, isVisible); // Invokes the OnVisiblityChanged event
        }

        public bool IsVisible()
        {
            return consoleView.activeSelf;
        }

        public void CycleAutoCompleteForward()
        {
            _autoCompleteIndex++;
            UpdateAutoCompleteText();
        }

        public void CycleAutoCompleteBackward()
        {
            _autoCompleteIndex--;
            UpdateAutoCompleteText();
        }

        public void MoveHistoryBackward()
        {
            if (InputHistory.TryMoveBackwards(out string prevCommand))
                inputFieldDisplay.text = prevCommand;
        }

        public void MoveHistoryForward()
        {
            if (InputHistory.TryMoveForwards(out string nextCommand))
                inputFieldDisplay.text = nextCommand;
        }

        // === Event Handlers ===
        private void HandleSubmit(string input)
        {
            string[] splitInput = input.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            if (input.Length > 0 && splitInput.Length > 0)
            {
                string[] args = ArgumentTools.Parse(splitInput);
                ICommand command = CommandRegistry.FindCommand(splitInput[0]);
                textDisplay.text += '\n' + userPrefix.GenerateMessage(input);

                command.Execute(args, this);
                InputHistory.AddEntry(input);
            }

            ResetInputField();
        }

        private void HandleVisibilityChange(bool wasVisible, bool isVisible)
        {
            if (isVisible)
                ResetInputField();
        }

        private void HandleInputChange(string value)
        {
            string[] splitInput = value.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            if (splitInput.Length == 0)
            {
                // case: we have no input.
                autoCompleteDisplay.text = string.Empty;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                int newSpaceCount = value.Count(c => c == ' ');
                int oldSpaceCount = _oldValue.Count(c => c == ' ');

                // If we entered a space, apply the auto-complete.
                if (_suggestions.Any() && newSpaceCount > oldSpaceCount)
                {
                    _autoCompleteIndex = 0;
                    string autoCompleteText = autoCompleteDisplay.text;
                    inputFieldDisplay.text = autoCompleteText + " ";
                    inputFieldDisplay.caretPosition = inputFieldDisplay.text.Length;
                }
            }

            CommandRegistry.FindCommands(inputFieldDisplay.text, _suggestions);
            UpdateAutoCompleteText();
            _oldValue = value;
        }

        // === Helpers ===
        private void UpdateAutoCompleteText()
        {
            int index = (int) Mathf.Repeat(_autoCompleteIndex, _suggestions.Count);
            string autoCompleteText = _suggestions.Count > 0 ? _suggestions[index] : "";
            autoCompleteDisplay.text = autoCompleteText;
        }

        private void ResetInputField()
        {
            inputFieldDisplay.text = string.Empty;
            inputFieldDisplay.ActivateInputField();
            InputHistory.Clear();
        }

        // === Structures ===
        /// <summary> <b>Description</b>: Custom Structure, w/ a RuntimeConsole Object </summary>
        public struct CreateEvent 
        {
            public RuntimeConsole Console;
        }
    }
}

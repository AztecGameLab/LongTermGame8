using System;
using System.Collections.Generic;
using System.Linq;
using poetools.Console.Commands;
using UnityEngine;
using UnityEngine.UI;

namespace poetools.Console
{
    public class RuntimeConsole : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The number of commands that should be remembered after running.")]
        private int maxCommandHistory = 30;

        [SerializeField]
        [Tooltip("Whether the console should begin opened or closed.")]
        private bool startVisible;

        [SerializeField]
        [Tooltip("The commands that are registered with the console by default.")]
        private Command[] autoRegisterCommands;

        [Header("Style")]

        [SerializeField]
        [Tooltip("The prefix that is shown before logged messages.")]
        private LogPrefix logPrefix;

        [SerializeField]
        [Tooltip("The prefix that is shown before used-entered messages.")]
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
        private GameObject consoleView;

        private List<ICommand> _commandInstances;
        private List<string> _suggestions;
        private int _autoCompleteIndex;
        private string _oldValue;

        public static event Action<CreateEvent> OnCreate;
        public event Action<bool, bool> OnVisibilityChanged;

        public CommandRegistry CommandRegistry { get; private set; }
        private IInputHistory InputHistory { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            OnCreate = null;
        }

        private void Awake()
        {
            CommandRegistry = new CommandRegistry();
            InputHistory = new InputHistory(maxCommandHistory);
            _commandInstances = new List<ICommand>();
            _suggestions = new List<string>();

            // Initialize the view.
            SetVisible(startVisible);

            // Register default commands.
            foreach (Command command in autoRegisterCommands)
            {
                Command instance = Instantiate(command);
                instance.Initialize(this);
                CommandRegistry.Register(instance);
                _commandInstances.Add(instance);
            }

            OnCreate?.Invoke(new CreateEvent{Console = this});
        }

        private void OnDestroy()
        {
            foreach (ICommand command in _commandInstances)
                command.Dispose();

            CommandRegistry.Dispose();
        }

        private void OnEnable()
        {
            OnVisibilityChanged += HandleVisibilityChange;
            inputFieldDisplay.onSubmit.AddListener(HandleSubmit);
            inputFieldDisplay.onValueChanged.AddListener(HandleInputChange);
        }

        private void OnDisable()
        {
            OnVisibilityChanged -= HandleVisibilityChange;
            inputFieldDisplay.onSubmit.RemoveListener(HandleSubmit);
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
        }

        public void SetVisible(bool isVisible)
        {
            bool wasVisible = consoleView.activeSelf;
            consoleView.SetActive(isVisible);
            OnVisibilityChanged?.Invoke(wasVisible, isVisible);
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
        public struct CreateEvent
        {
            public RuntimeConsole Console;
        }
    }
}

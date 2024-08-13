using System;
using System.Collections.Generic;
using poetools.Console.Commands;

namespace poetools.Console
{
    /// <summary>
    /// The structure responsible for tracking and finding all registered commands.
    /// It can also supply autocompletion for a given string.
    /// </summary>
    public class CommandRegistry : IDisposable
    {
        private readonly Dictionary<string, ICommand> _commandLookup; // = new Dictionary<string, ICommand>();
        private readonly DictionaryTree _autoCompleter; // = new DictionaryTree();

        public ICommand DefaultCommand { get; } = new ErrorCommand();
        public IEnumerable<ICommand> AllCommands => _commandLookup.Values;

        // NOTE 1: Action<CommandAddEvent> refers to a method that takes CommandAddEvent as a parameter
        // NOTE 2: In order for the event to not be "Null", something has to subscribe to the event, like CommandAdded += HandleCommandAdded
        
        /// <summary> <b>Description</b>: A custom event for when a new command is added. Other methods can subscribe to this event </summary>
        public event Action<CommandAddEvent> CommandAdded;
        
        /// <summary> <b>Description</b>: A custom event for when a command is removed. Other methods can subscribe to this event </summary>
        public event Action<CommandRemoveEvent> CommandRemoved;

        /// <summary> <b>Description</b>: Disposes of (deconstructs) the command registry object </summary>
        public void Dispose()
        {
            DefaultCommand?.Dispose();
        }

        // === Registration ===
        /// <summary>
        ///   <b>Description</b>: This method writes the game dialogue, revealings the characters
        ///     one by one to create the text-typing effect.
        /// </summary>
        /// <param name="text"> The dialogue text to display </param>
        public void Register(params ICommand[] commandList) // NOTE: Params allows a variable (not fixed) number of parameters 
        {
            foreach (var command in commandList) // For every command in the parameters
            {
                _commandLookup.Add(command.Name, command); // Add the command to the Command Dictionary
                _autoCompleter.Insert(command.Name); // Adds the command to the auto-completer Dictionary Tree

                AddAutoCompletions(command.AutoCompletions); // Adds the commands auto-completions to the auto-completer
                // Invokes CommandAdded, with a CommandAddEvent argument that has "command" as the ICommand object (only if it has subscribers)
                CommandAdded?.Invoke(new CommandAddEvent{Command = command}); 
            }
        }

        public void Unregister(params ICommand[] commandList)
        {
            foreach (var command in commandList)
            {
                _commandLookup.Remove(command.Name);
                _autoCompleter.Remove(command.Name);

                RemoveAutoCompletions(command.AutoCompletions);
                CommandRemoved?.Invoke(new CommandRemoveEvent{Command = command});
            }
        }

        // === Auto Completion ===
        public void RemoveAutoCompletions(IEnumerable<string> autoCompletions)
        {
            foreach (var autoCompletion in autoCompletions)
                RemoveAutoCompletion(autoCompletion);
        }

        
        /// <summary> <b>Description</b>: Adds multiple auto-completions to the autoCompleter Dictionary Tree </summary>
        public void AddAutoCompletions(IEnumerable<string> autoCompletions)
        {
            foreach (var autoCompletion in autoCompletions)
                AddAutoCompletion(autoCompletion);
        }
        
        public void RemoveAutoCompletion(string autoCompletion)
        {
            _autoCompleter.Remove(autoCompletion);
        }

        /// <summary> <b>Description</b>: Adds an auto-completion to the autoCompleter Dictionary Tree </summary>
        public void AddAutoCompletion(string autoCompletion) 
        {
            _autoCompleter.Insert(autoCompletion); // Insert an autoCompletion into the autoCompleter list
        }

        // === Searching ===
        public void FindCommands(string commandName, List<string> results)
        {
            results.Clear();

            if (commandName.Length == 0)
                return;

            _autoCompleter.GetWithPrefix(commandName, results);

            for (int i = 0; i < results.Count; i++)
                results[i] = commandName + results[i];
        }

        public ICommand FindCommand(string commandName)
        {
            if (!_commandLookup.TryGetValue(commandName, out var result))
                result = DefaultCommand;

            return result;
        }

        // === Structures ===
        /// <summary> <b>Description</b>: Custom Structure, w/ a ICommand Object </summary>
        public struct CommandAddEvent
        {
            public ICommand Command;
        }

        /// <summary> <b>Description</b>: Custom Structure, w/ a ICommand Object </summary>
        public struct CommandRemoveEvent
        {
            public ICommand Command;
        }
    }
}

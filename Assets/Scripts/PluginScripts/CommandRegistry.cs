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
        /// <summary> <b>Description</b>: A dictionary, where a string (the name of the command) is the key
        /// and the Command you are looking for is the value </summary>
        private readonly Dictionary<string, ICommand> _commandLookup; // = new Dictionary<string, ICommand>();
        
        /// <summary> <b>Description</b>: A DictionaryTree used for the auto-completer in the console. When
        /// you begin typing a command, the autoCompleter DictionaryTree finds words that begin with the
        /// user-given prefix. </summary>
        private readonly DictionaryTree _autoCompleter; // = new DictionaryTree();

        /// <summary> <b>Description</b>: The Default Command (runs when no existing command is called) is set
        /// to the Error Command </summary>
        public ICommand DefaultCommand { get; } = new ErrorCommand();
        
        /// <summary> <b>Description</b>: An IEnumerable set to all the values of the command-lookup Dictionary. This
        /// holds all existing commands. </summary>
        public IEnumerable<ICommand> AllCommands => _commandLookup.Values;

        // NOTE 1: Action<CommandAddEvent> refers to a method that takes CommandAddEvent as a parameter
        // NOTE 2: In order for the event to not be "Null", something has to subscribe to the event, like CommandAdded += HandleCommandAdded
        
        /// <summary> <b>Description</b>: A custom event for when a new command is added. Other methods can subscribe to this event </summary>
        public event Action<CommandAddEvent> CommandAdded;
        
        /// <summary> <b>Description</b>: A custom event for when a command is removed. Other methods can subscribe to this event </summary>
        public event Action<CommandRemoveEvent> CommandRemoved;

        /// <summary> <b>Description</b>: Disposes of (deconstructs) the Default Command </summary>
        public void Dispose()
        {
            DefaultCommand?.Dispose();
        }

        // === Registration ===
        /// <summary>
        ///   <b>Description</b>: This method registers the game commands by adding them to the necessary
        /// dictionaries
        /// </summary>
        /// <param name="commandList"> Every command in the parameters, can be iterated through </param>
        public void Register(params ICommand[] commandList) // NOTE: Params allows a variable (not fixed) number of parameters 
        {
            foreach (var command in commandList) // For every command in the parameters
            {
                // Command.Name gives the string field that represents the command
                _commandLookup.Add(command.Name, command); // Add the command to the Command Dictionary
                _autoCompleter.Insert(command.Name); // Adds the command to the auto-completer Dictionary Tree

                AddAutoCompletions(command.AutoCompletions); // Adds the command's auto-completions to the auto-completer Dictionary Tree
                // Invokes CommandAdded, with a CommandAddEvent argument that has "command" as the ICommand object (only if it has subscribers)
                CommandAdded?.Invoke(new CommandAddEvent{Command = command}); 
            }
        }

        /// <summary>
        ///   <b>Description</b>: This method unregisters the game commands by removed them from the necessary
        /// dictionaries
        /// </summary>
        /// <param name="commandList"> Every command in the parameters, can be iterated through </param>
        public void Unregister(params ICommand[] commandList)
        {
            foreach (var command in commandList) // For every command in the parameters
            {
                _commandLookup.Remove(command.Name); // Remove the command from the Command Dictionary
                _autoCompleter.Remove(command.Name); // Remove the command from the auto-completer Dictionary Tree

                RemoveAutoCompletions(command.AutoCompletions); // Remove the command's autocompletions from the auto-completer
                // Invokes CommandRemoved, with a CommandRemoveEvent argument that has "command" as its ICommand object
                CommandRemoved?.Invoke(new CommandRemoveEvent{Command = command});
            }
        }

        // === Auto Completion ===
        /// <summary> <b>Description</b>: Removes multiple auto-completions from the autoCompleter Dictionary Tree </summary>
        /// <param name="autoCompletions"> The auto-completion(s) to remove from the auto-completer </param>
        public void RemoveAutoCompletions(IEnumerable<string> autoCompletions)
        {
            foreach (var autoCompletion in autoCompletions)
                RemoveAutoCompletion(autoCompletion);
        }
        
        /// <summary> <b>Description</b>: Adds multiple auto-completions to the autoCompleter Dictionary Tree </summary>
        /// <param name="autoCompletions"> The auto-completion(s) to add to the auto-completer </param>
        public void AddAutoCompletions(IEnumerable<string> autoCompletions)
        {
            foreach (var autoCompletion in autoCompletions)
                AddAutoCompletion(autoCompletion);
        }
        
        /// <summary> <b>Description</b>: Removes an auto-completion from the autoCompleter Dictionary Tree </summary>
        /// <param name="autoCompletion"> The auto-completion to remove from the auto-completer </param>
        public void RemoveAutoCompletion(string autoCompletion)
        {
            _autoCompleter.Remove(autoCompletion);
        }

        /// <summary> <b>Description</b>: Adds an auto-completion to the autoCompleter Dictionary Tree </summary>
        /// <param name="autoCompletion"> The auto-completion to add to the auto-completer </param>
        public void AddAutoCompletion(string autoCompletion) 
        {
            _autoCompleter.Insert(autoCompletion); // Insert an autoCompletion into the autoCompleter list
        }

        // === Searching ===
        /// <summary>
        ///   <b>Description</b>: This method takes in a List of strings, empties it, and then uses a given command (prefix)
        /// to find all "auto-completed" commands that start with that prefix, filling the given List of strings with these commands
        /// </summary>
        /// <param name="commandName"> The name of the command to find auto-completions for </param>
        /// <param name="results"> The list where found auto-completed commands are inputted into </param>
        public void FindCommands(string commandName, List<string> results)
        {
            results.Clear(); // Removes all items from the "results" List

            if (commandName.Length == 0) // If the commandName string is empty, return
                return;

            _autoCompleter.GetWithPrefix(commandName, results); /* Searches for any commands with the prefix given
            in "commandName", and then inserts the given command into the results list. */

            // For every value in "results"
            for (int i = 0; i < results.Count; i++)
                results[i] = commandName + results[i]; /* The entry at that index is set to the command name + 
                the previous entry at that index */
        }

        
        public ICommand FindCommand(string commandName)
        {
            // Attempts to find a value using the given key ("commandName"), puts the value in "result" if found
            if (!_commandLookup.TryGetValue(commandName, out var result))
                // otherwise, result is set to the DefaultCommand
                result = DefaultCommand;

            return result; // Returns the result
        }

        // === Structures ===
        /// <summary> <b>Description</b>: Custom Structure, w/ an ICommand Object </summary>
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

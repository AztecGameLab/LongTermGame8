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
        private readonly Dictionary<string, ICommand> _commandLookup = new Dictionary<string, ICommand>();
        private readonly DictionaryTree _autoCompleter = new DictionaryTree();

        public ICommand DefaultCommand { get; } = new ErrorCommand();
        public IEnumerable<ICommand> AllCommands => _commandLookup.Values;

        public event Action<CommandAddEvent> CommandAdded;
        public event Action<CommandRemoveEvent> CommandRemoved;

        public void Dispose()
        {
            DefaultCommand?.Dispose();
        }

        // === Registration ===
        public void Register(params ICommand[] commandList)
        {
            foreach (var command in commandList)
            {
                _commandLookup.Add(command.Name, command);
                _autoCompleter.Insert(command.Name);

                AddAutoCompletions(command.AutoCompletions);
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

        public void AddAutoCompletions(IEnumerable<string> autoCompletions)
        {
            foreach (var autoCompletion in autoCompletions)
                AddAutoCompletion(autoCompletion);
        }

        public void RemoveAutoCompletion(string autoCompletion)
        {
            _autoCompleter.Remove(autoCompletion);
        }

        public void AddAutoCompletion(string autoCompletion)
        {
            _autoCompleter.Insert(autoCompletion);
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
        public struct CommandAddEvent
        {
            public ICommand Command;
        }

        public struct CommandRemoveEvent
        {
            public ICommand Command;
        }
    }
}

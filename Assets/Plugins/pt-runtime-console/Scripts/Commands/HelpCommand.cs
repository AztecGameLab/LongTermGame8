using System;
using System.Collections.Generic;
using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Help")]
    public class HelpCommand : Command
    {
        public override string Name => "help";
        public override string Help => "Print information on how to use other commands!";

        public override IEnumerable<string> AutoCompletions => Array.Empty<string>();

        private CommandRegistry _commandRegistry;

        public override void Initialize(RuntimeConsole console)
        {
            _commandRegistry = console.CommandRegistry;
            _commandRegistry.CommandAdded += HandleCommandAdded;
            _commandRegistry.CommandRemoved += HandleCommandRemoved;
        }

        public override void Dispose()
        {
            if (_commandRegistry != null)
            {
                _commandRegistry.Dispose();
                _commandRegistry.CommandAdded -= HandleCommandAdded;
                _commandRegistry.CommandRemoved -= HandleCommandRemoved;
            }
        }

        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length != 1)
            {
                console.Log("help", "List of all available commands: ");
                
                foreach (ICommand c in _commandRegistry.AllCommands)
                    console.LogRaw($"\t{c.Name} - {c.Help}");
            }
            else
            {
                ICommand command = _commandRegistry.FindCommand(args[0]);
                console.Log("help", $"\n{command.Help}");
            }
        }

        private void HandleCommandAdded(CommandRegistry.CommandAddEvent eventData)
        {
            _commandRegistry.AddAutoCompletion($"help {eventData.Command.Name}");
        }

        private void HandleCommandRemoved(CommandRegistry.CommandRemoveEvent eventData)
        {
            _commandRegistry.RemoveAutoCompletion($"help {eventData.Command.Name}");
        }
    }
}

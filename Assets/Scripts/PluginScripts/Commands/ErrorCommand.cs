using System;
using System.Collections.Generic;
using PluginScripts;

namespace poetools.Console.Commands
{
    /// <summary>
    /// The structure responsible for tracking and finding all registered commands.
    /// It can also supply autocompletion for a given string.
    /// </summary>
    public class ErrorCommand : ICommand
    {
        // Sets the Name field to an empty string 
        public string Name => string.Empty;
        
        // Sets the Help field to an empty string 
        public string Help => string.Empty;
        
        // Sets the Auto-Completions IEnumerable to an Empty Array
        public IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>();

        /// <summary> <b>Description</b>: The Execution for the command, or what happens when it's called. In this case,
        /// this command runs when there is an error in the console, specifically, someone typing a command that doesn't
        /// exist. </summary>
        /// <param name="args"> The arguments given with the command </param>
        /// <param name="console"> The console the command was run on </param>
        public void Execute(string[] args, RuntimeConsole console)
        {
            string message = "<color=grey><i>Invalid command! Try using \"help [command]\" for proper usage.</i></color>";
            console.LogRaw($"\n{message}");
        }

        /// <summary> <b>Description</b>: No dispose function is needed for this command </summary>
        public void Dispose()
        {
            // Do nothing.
        }
    }
}

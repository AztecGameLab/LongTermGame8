using System;
using System.Collections.Generic;

namespace poetools.Console.Commands
{
    public class ErrorCommand : ICommand
    {
        public string Name => string.Empty;
        public string Help => string.Empty;
        public IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>();

        public void Execute(string[] args, RuntimeConsole console)
        {
            string message = "<color=grey><i>Invalid command! Try using \"help [command]\" for proper usage.</i></color>";
            console.LogRaw($"\n{message}");
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}

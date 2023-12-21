using System;
using System.Collections.Generic;

namespace poetools.Console.Commands
{
    public class NullCommand : ICommand
    {
        public string Name => string.Empty;
        public string Help => string.Empty;
        public IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>();

        public void Execute(string[] args, RuntimeConsole console)
        {
            // Do nothing.
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}

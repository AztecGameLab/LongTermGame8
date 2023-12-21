using System.Collections.Generic;
using UnityEngine;

namespace poetools.Console.Commands
{
    // todo: sens, invert y, help
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Mouse")]
    public class MouseCommand : Command
    {
        private RuntimeConsole _console;

        public override string Name => "mouse";
        public override string Help => "Allows you to edit the cursor settings.";

        public override IEnumerable<string> AutoCompletions { get; } = new[]
        {
            "mouse show", "mouse hide",
        };

        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length >= 1)
            {
                _console = console;

                switch (args[0])
                {
                    case "show": HandleShow(); break;
                    case "hide": HandleHide(); break;
                }
            }
        }

        private void HandleShow()
        {
            _console.Log(Name, "Mouse has been made visible.");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void HandleHide()
        {
            _console.Log(Name, "Mouse has been made invisible.");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

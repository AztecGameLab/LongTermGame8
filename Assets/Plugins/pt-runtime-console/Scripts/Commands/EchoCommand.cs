using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Echo")]
    public class EchoCommand : Command
    {
        public override string Name => "echo";
        public override string Help => "Prints a message onto the console.";

        public override void Execute(string[] args, RuntimeConsole console)
        {
            string message = "";

            foreach (string arg in args)
                message += arg + " ";

            console.Log(Name, message);
        }
    }
}

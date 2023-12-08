using poetools.Console;
using poetools.Console.Commands;
using UnityEngine;
namespace Commands
{
    [CreateAssetMenu]
    public class SaveCommands : Command
    {
        public override string Name => "save";
        
        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length == 0)
                return;

            Ltg8SaveSystem save = Ltg8.Save;
            
            if (args[0] == "var" && args.Length == 3)
            {
                switch (args[1])
                {
                    case "get":
                        console.Log("save", save.GetVar(args[2]).ToString());
                        break;
                    case "set" when args.Length == 4 && int.TryParse(args[3], out int value):
                        save.SetVar(args[2], value);
                        break;
                }
            }
            if (args[0] == "flag" && args.Length == 3)
            {
                switch (args[1])
                {
                    case "get":
                        console.Log("save", save.GetFlag(args[2]).ToString());
                        break;
                    case "set":
                        save.SetFlag(args[2]);
                        break;
                    case "reset":
                        save.ResetFlag(args[2]);
                        break;
                }
            }
        }
    }
}

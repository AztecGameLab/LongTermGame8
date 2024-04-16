using poetools.Console;
using poetools.Console.Commands;
using UnityEngine;

namespace PluginScripts.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/FunnyCommand1")]
    public class FunnyCommand1 : Command
    {
        // Determines the name and help message for the command in the console
        public override string Name => "amogus";
        public override string Help => "";
         
        // The player-character GameObject
        private GameObject _events;

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy
            console.Log(Name, "Sigmund was sus :(");
            _events = GameObject.Find("EventSystem");
            _events.GetComponent<UnhideAmogus>().UnhideAmogusScript();
        }
    }
}

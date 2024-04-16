using poetools.Console;
using poetools.Console.Commands;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace PluginScripts.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/FunnyCommand2")]
    public class FunnyCommand2 : Command
    {
        // Determines the name and help message for the command in the console
        public override string Name => "markiplier";
        public override string Help => "";
         
        // The player-character GameObject
        private GameObject _player;

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy
            console.Log(Name,"Hellooooo Everybody, my name is Sigmundplier");
            _player = GameObject.Find("Player");
            _player.GetComponent<Moustache>().GiveMoustache();
        }
    }
}

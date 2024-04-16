using System;
using poetools.Console;
using poetools.Console.Commands;
using UnityEngine;
using Player;

namespace PluginScripts.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/IgnoreDialogue")]
    public class SensitivityCommand : Command
    {
        // Determines the name and help message for the command in the console
        public override string Name => "sensitivity";
        public override string Help => "Changes the mouse sensitivity";
        
        // The player-character GameObject
        private GameObject _player;

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy 
            _player = GameObject.Find("Player");
            
            console.Log(name, "Changing the mouse sensitivity to " + args[0] + ".");
            
            // Calls SensitivityChanger to c
            _player.GetComponent<PlayerController>().ChangePlayerSensitivity(Convert.ToSingle(args[0]));
        }
    }
}


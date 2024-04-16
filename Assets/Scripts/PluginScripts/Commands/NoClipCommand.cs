using PluginScripts;
using poetools.Console;
using poetools.Console.Commands;
using poetools.PluginScripts.Executions;
using UnityEngine;

namespace poetools.PluginScripts.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/NoClip")]
    public class NoClipCommand : Command
    {
        // Determines the name and help message for the command in the console
        public override string Name => "noclip";
        public override string Help => "Allows you to fly and phase through the map with ease.";
        
        // The player-character GameObject
        private GameObject _player;
        
        // Is no-clipping enabled or disabled?
        private bool _noClip;

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy 
            _player = GameObject.Find("Player");
            
            if (_noClip)
            {
                console.Log(Name, "noclip disabled");
                _noClip = false;
                TogglePhysics();
            }
            else
            {
                console.Log(Name, "noclip enabled");
                _noClip = true;
                TogglePhysics();
            }
        }

        // Disables gravity (if enabled) and calls the NoClipMovement script to activate no-clipping
        private void TogglePhysics()
        {
            _player.GetComponent<NoClipMovement>().ToggleNoclip();
            _player.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}

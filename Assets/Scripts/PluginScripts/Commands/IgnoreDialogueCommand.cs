using PluginScripts;
using poetools.Console;
using poetools.Console.Commands;
using poetools.PluginScripts.Executions;
using UnityEngine;

namespace poetools.PluginScripts.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/IgnoreDialogue")]
    public class IgnoreDialogueCommand : Command
    {
        // Determines the name and help message for the command in the console
        public override string Name => "ignoredialogue";
        public override string Help => "Prevents you from activating dialogue triggers";
         
        // The player-character GameObject
        private GameObject _player;
        
        // Is dialogue set to be ignored?
        private bool _dialogueToggle;

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy
            _player = GameObject.Find("Player");
            
            // Calls on IgnoreDialogue to activate or deactivate the ignore dialogue option
            _player.transform.GetComponent<IgnoreDialogue>().ToggleIgnoreDialogue();
            
            if (_dialogueToggle)
            {
                _dialogueToggle = false;
                console.Log(Name, "Dialogue Triggers Activated");
            }
            else
            {
                _dialogueToggle = true;
                console.Log(Name, "Dialogue Triggers Deactivated");
            }
        }
    }
}

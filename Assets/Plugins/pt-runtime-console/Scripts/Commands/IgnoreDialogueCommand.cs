using System;
using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/IgnoreDialogue")]
    public class IgnoreDialogueCommand : Command
    {
        public override string Name => "ignoredialogue";
        public override string Help => "Prevents you from activating dialogue triggers";
         
        private GameObject _player;
        private bool _dialogueToggle;

        public override void Execute(string[] args, RuntimeConsole console)
        {
            _player = GameObject.Find("Player");
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

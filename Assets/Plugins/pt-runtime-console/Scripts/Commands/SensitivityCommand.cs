using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/IgnoreDialogue")]
    public class SensitivityCommand : Command
    {
        public override string Name => "sensitivity";
        public override string Help => "Changes the mouse sensitivity";

        [SerializeField] private ScriptableObject sensitivityChanger;
         
        private GameObject _player;

        public override void Execute(string[] args, RuntimeConsole console)
        {
            
            console.Log(name, "Changing the mouse sensitivity to " + args[0] + ".");
        }
    }
}


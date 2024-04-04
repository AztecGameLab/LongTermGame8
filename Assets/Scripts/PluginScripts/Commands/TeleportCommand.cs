using System;
using PluginScripts;
using poetools.Console;
using poetools.Console.Commands;
using UnityEngine;

namespace poetools.PluginScripts.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Teleport")]
    public class TeleportCommand : Command
    {
        // Determines the name and help message for the command in the console
        public override string Name => "teleport";
        public override string Help => "Allows you teleport to specified areas or coords";
        
        
        // The player-character GameObject
        private GameObject _player;

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy
            _player = GameObject.Find("Player");
            
            switch (args.Length)
            {
                // If there is (1) argument, teleport to the given pre-set location
                case 1:
                    console.Log(Name,"Teleporting to '" + args[0] + "'");
                    switch (args[0])
                    {
                        case "catapult":
                        {
                            _player.transform.position = new Vector3(32, 18, 35);
                            return;
                        }
                        case "door":
                        {
                            _player.transform.position = new Vector3(32, 7, -36);
                            return;
                        }
                        case "boulder":
                        {
                            _player.transform.position = new Vector3(41, 16, 65);
                            return;
                        }
                        case "trunks":
                        {
                            _player.transform.position = new Vector3(52, 5, 97);
                            return;
                        }
                        case "light":
                        {
                            _player.transform.position = new Vector3(79, 16, 86);
                            return;
                        }
                        case "platforms":
                        {
                            _player.transform.position = new Vector3(-31, 24, 73);
                            return;
                        }
                        case "tent":
                        {
                            _player.transform.position = new Vector3(96, 16, -3);
                            return;
                        }
                        case "camera":
                        {
                            _player.transform.position = new Vector3(-41, 16, -10);
                            return;
                        }
                        case "rope":
                        {
                            _player.transform.position = new Vector3(5, 9, 65);
                            return;
                        }
                        case "axe":
                        {
                            _player.transform.position = new Vector3(-6, 14, -29);
                            return;
                        }
                    }

                    break;
                // If there are (3) arguments, teleport to the given coordinates
                case 3:
                    _player.transform.position = new Vector3(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]), Convert.ToSingle(args[2]));
                    console.Log(Name, "Teleporting to (" + args[0] + ", " + args[1] + ", " + args[2] + ")");
                    break;
            }
        }
    }
}

using System;
using PluginScripts;
using poetools.Console;
using poetools.Console.Commands;
using UnityEditor;
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
        
        private CommandRegistry _commandRegistry;

        private string[] _locations;

        public override void Initialize(RuntimeConsole console)
        {
            _commandRegistry = console.CommandRegistry;
            _locations = new[] { "catapult", "door", "boulder", "trunks", "light", "platforms", "tent", "camera", "rope", "axe" };
            _commandRegistry.CommandAdded += HandleCommandAdded;
            _commandRegistry.CommandRemoved += HandleCommandRemoved;
        }

        public override void Dispose()
        {
            if (_commandRegistry == null) return;
            _commandRegistry.Dispose();
            _commandRegistry.CommandAdded -= HandleCommandAdded;
            _commandRegistry.CommandRemoved -= HandleCommandRemoved;
        }

        // Command Execution
        public override void Execute(string[] args, RuntimeConsole console)
        {
            // Finds the player GameObject in the heirarchy
            _player = GameObject.Find("Player");
            
            switch (args.Length)
            {
                // If there is (1) argument, teleport to the given pre-set location
                case 1:
                    switch (args[0].ToLower())
                    {
                        case "catapult":
                        {
                            _player.transform.position = new Vector3(32, 18, 35);
                            break;
                        }
                        case "door":
                        {
                            _player.transform.position = new Vector3(32, 7, -36);
                            break;
                        }
                        case "boulder":
                        {
                            _player.transform.position = new Vector3(41, 16, 65);
                            break;
                        }
                        case "trunks":
                        {
                            _player.transform.position = new Vector3(52, 5, 97);
                            break;
                        }
                        case "light":
                        {
                            _player.transform.position = new Vector3(79, 16, 86);
                            break;
                        }
                        case "platforms":
                        {
                            _player.transform.position = new Vector3(-31, 24, 73);
                            break;
                        }
                        case "tent":
                        {
                            _player.transform.position = new Vector3(96, 16, -3);
                            break;
                        }
                        case "camera":
                        {
                            _player.transform.position = new Vector3(-41, 16, -10);
                            break;
                        }
                        case "rope":
                        {
                            _player.transform.position = new Vector3(5, 9, 65);
                            break;
                        }
                        case "axe":
                        {
                            _player.transform.position = new Vector3(-6, 14, -29);
                            break;
                        } 
                        default:
                            console.Log(Name, "Please input a valid location name");
                            return;
                    }
                    console.Log(Name,"Teleporting to '" + args[0] + "'");
                    break;
                // If there are (2) arguments, let the user know the error.
                case 2:
                    console.Log(Name, "Not enough arguments");
                    return;
                // If there are more than (3) arguments, teleport to the coordinates given by the first 3 arguments
                case >=3:
                    if(!(int.TryParse(args[0], out _) && int.TryParse(args[1], out _) && int.TryParse(args[2], out _)))
                    {
                        console.Log(Name, "Please enter valid integers or floats");
                        return;
                    }
                    _player.transform.position = new Vector3(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]), Convert.ToSingle(args[2]));
                    console.Log(Name, "Teleporting to (" + args[0] + ", " + args[1] + ", " + args[2] + ")");
                    break;
            }
        }
        
        private void HandleCommandAdded(CommandRegistry.CommandAddEvent commandAddEvent)
        {
            foreach (var location in _locations)
            {
                _commandRegistry.AddAutoCompletion($"teleport {location}");
            }
            
        }

        private void HandleCommandRemoved(CommandRegistry.CommandRemoveEvent eventData)
        {
            foreach (var location in _locations)
            {
                _commandRegistry.RemoveAutoCompletion($"teleport {location}");
            }
        }
    }
}

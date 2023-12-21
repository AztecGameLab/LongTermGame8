using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Quit")]
    public class QuitCommand : Command
    {
        public override string Name => "quit";
        public override string Help => "Stops the application.";

        public override void Execute(string[] args, RuntimeConsole console)
        {
            console.Log(Name, "Quitting game.");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

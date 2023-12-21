using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Debug")]
    public class DebugCommand : Command
    {
        public override string Name => "debug";

        private ConsoleDebugManager _debugManager;

        private static List<IConsoleDebugInfo> GetAllDebugInfo()
        {
            List<IConsoleDebugInfo> result = new List<IConsoleDebugInfo>();
            
            foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                result.AddRange(gameObject.GetComponentsInChildren<IConsoleDebugInfo>());

            return result;
        }
        
        public override void Initialize(RuntimeConsole console)
        {
            _debugManager = FindAnyObjectByType<ConsoleDebugManager>();
        }

        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length == 0)
            {
                foreach (IConsoleDebugInfo debugInfo in GetAllDebugInfo())
                    console.LogRaw(debugInfo.DebugName);
            }
            else
            {
                string targetName = args[0];
                
                for (int i = 1; i < args.Length; ++i)
                    targetName += ' ' + args[i];
                
                GameObject instance = GameObject.Find(targetName);
                
                if (instance != null && instance.TryGetComponent(out IConsoleDebugInfo debugInfo))
                    _debugManager.Register(debugInfo);
            }
        }
    }

}

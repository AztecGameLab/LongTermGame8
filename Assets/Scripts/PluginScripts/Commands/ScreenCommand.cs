using System.Collections.Generic;
using PluginScripts;
using poetools.PluginScripts;
using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Screen")]
    public class ScreenCommand : Command
    {
        public override string Name => "screen";
        public override string Help => "Allows you to change various options of the window, such as resolution or refresh rate.";

        public override IEnumerable<string> AutoCompletions => new[]
        {
            "screen max", "screen exclusive", "screen windowed",
            "screen resolution", "screen refresh", "screen msaa", "screen fov",
            "screen vsync", "screen vsync true", "screen vsync false",
        };

        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length >= 1)
            {
                switch (args[0])
                {
                    case "max":
                        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                        break;
                    case "exclusive":
                        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        break;
                    case "windowed":
                        Screen.fullScreenMode = FullScreenMode.Windowed;
                        break;
                    case "resolution":
                        if (args.Length >= 3 && int.TryParse(args[1], out int width) && int.TryParse(args[2], out int height))
                            Screen.SetResolution(width, height, Screen.fullScreenMode);
                        else console.Log("screen", $"{Screen.width}x{Screen.height}");
                        break;
                    case "refresh":
                        if (args.Length >= 2 && int.TryParse(args[1], out int rate))
                        {
#if UNITY_2022_3
                            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, new RefreshRate{numerator = rate, denominator = 1});
#else
                            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, rate);
#endif
                        }
                        else console.Log("screen", $"{Screen.currentResolution.refreshRate} HZ");
                        break;
                    case "vsync":
                        if (args.Length >= 2 && bool.TryParse(args[1], out bool vsync))
                        {
                            QualitySettings.vSyncCount = vsync ? 1 : 0;
                        }
                        else console.Log("screen", $"VSYNC: {(QualitySettings.vSyncCount == 1 ? "On" : "Off")}");
                        break;
                    case "fov":
                        Camera mainCamera = Camera.main;

                        if (mainCamera != null)
                        {
                            if (args.Length >= 2 && int.TryParse(args[1], out int fov))
                                mainCamera.fieldOfView = fov;
                            else console.Log("screen", $"{mainCamera.fieldOfView}");
                        }
                        
                        break;
                }
            }
        }
    }
}

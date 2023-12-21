using System;
using poetools.Console.Commands;
using UnityEngine;

namespace poetools.Console
{
    /// <summary>
    /// An attribute that allows commands to be automatically registered with a console when created.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterCommandAttribute : Attribute {}

    public static class AutoCommandRegister
    {
        // Note: this must run after Subsystems, since that is when RuntimeConsole resets stuff.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            RuntimeConsole.OnCreate += AutoRegister;
        }

        private static void AutoRegister(RuntimeConsole.CreateEvent eventData)
        {
            var commandRegistry = eventData.Console.CommandRegistry;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    if (typeof(ICommand).IsAssignableFrom(type) && type.GetCustomAttributes(typeof(AutoRegisterCommandAttribute), true).Length > 0)
                    {
                        var instance = Activator.CreateInstance(type) as ICommand;
                        commandRegistry.Register(instance);
                    }
                }
            }
        }
    }
}

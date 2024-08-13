using System;
using System.Collections.Generic;
using PluginScripts;
using UnityEngine;

namespace poetools.Console.Commands
{
    /// <summary>
    ///   <b>Description</b>: Interface for console commands
    /// <list type="table">
    ///       <item>
    ///           <description><b>Name</b> - Name of the command </description>
    ///       </item>
    ///       <item>
    ///           <description><b>Help</b> - The help message for the command </description>
    ///       </item>
    ///       <item>
    ///           <description><b>AutoCompletions</b> - List of sub-commands that can be auto-completed from this one </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface ICommand : IDisposable
    {
        string Name { get; }
        string Help { get; }
        IEnumerable<string> AutoCompletions { get; }

        /// <summary>
        ///   <b>Description</b>: Executes the given command
        /// </summary>
        /// <param name="category"> args </param>
        void Execute(string[] args, RuntimeConsole console);
    }

    /// <summary>
    ///   <b>Description</b>: Abstract class for console commands (Implements ICommand)
    /// <list type="table">
    ///       <item>
    ///           <description><b>Initialize()</b> - Initializes variables (if needed) </description>
    ///       </item>
    ///       <item>
    ///           <description><b>Dispose()</b> - Kind of like a deconstructor, releases unused stuff </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public abstract class Command : ScriptableObject, ICommand
    {
        public abstract string Name { get; }
        public virtual string Help => "<i><color=grey>No Help Available</color></i>"; // Default message for help sub-command, unless overriden 
        public virtual IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>(); // Default AutoCompletions list is empty, unless overriden
        public virtual void Initialize(RuntimeConsole console) {}
        public abstract void Execute(string[] args, RuntimeConsole console);
        public virtual void Dispose() {}
    }
}

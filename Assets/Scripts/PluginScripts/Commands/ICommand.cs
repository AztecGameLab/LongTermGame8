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
        /// <summary> <b>Description</b>: Name of the command </summary>
        string Name { get; }
        /// <summary> <b>Description</b>: Help Description for the command </summary>
        string Help { get; }
        /// <summary> <b>Description</b>: List of auto-completions for the command </summary>
        IEnumerable<string> AutoCompletions { get; }

        /// <summary>
        ///   <b>Description</b>: Code to execute when the command is called
        /// </summary>
        /// <param name="args"> The command arguments </param>
        /// <param name="console"> The Runtime Console the command is called on </param>
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
        /// <summary> <b>Description</b>: Name of the command </summary>
        public abstract string Name { get; }
        /// <summary> <b>Description</b>: Help description for the command </summary>
        public virtual string Help => "<i><color=grey>No Help Available</color></i>"; // Default message for help sub-command, unless overriden 
        /// <summary> <b>Description</b>: List of auto-completions for the command </summary>
        public virtual IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>(); // Default AutoCompletions list is empty, unless overriden
        /// <summary> <b>Description</b>: Code to execute upon initialization of the command </summary>
        public virtual void Initialize(RuntimeConsole console) {}
        /// <summary> <b>Description</b>: Code to execute once the command is called </summary>
        public abstract void Execute(string[] args, RuntimeConsole console);
        /// <summary> <b>Description</b>: Code to execute once the command is disposed of </summary>
        public virtual void Dispose() {}
    }
}

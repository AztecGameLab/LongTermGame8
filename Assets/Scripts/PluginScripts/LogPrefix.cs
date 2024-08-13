using UnityEngine;

namespace poetools.Console
{
    /// <summary>
    ///   <b>Description</b>: Abstract class for the generation of a Log Prefix [String]
    /// <list type="table">
    ///       <item>
    ///           <description><b>GenerateMessage()</b> - Takes in a string to generate the Log Prefix </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public abstract class LogPrefix : ScriptableObject
    {
        /// <summary>
        ///   <b>Description</b>: Generates the Log Prefix using the provided category
        /// </summary>
        /// <param name="category"> The category of the logged message </param>
        public abstract string GenerateMessage(string category);
    }
}

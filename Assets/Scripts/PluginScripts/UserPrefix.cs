using UnityEngine;

namespace poetools.Console
{
    /// <summary>
    ///   <b>Description</b>: Abstract class for the generation of a User Prefix [String]
    /// <list type="table">
    ///       <item>
    ///           <description><b>GenerateMessage()</b> - Takes in a string to generate the User Prefix </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public abstract class UserPrefix : ScriptableObject
    {
        /// <summary>
        ///   <b>Description</b>: Generates the User Prefix using the provided string
        /// </summary>
        /// <param name="input"> The string for the User Prefix </param>
        public abstract string GenerateMessage(string input);
    }
}
using FMODUnity;
using Plugins.FMOD.src;
using UnityEngine;

// Description: This class determines the way dialogue text is presented/revealed

namespace TextPresentation
{
    
    /// <summary>
    ///   <b>Description</b>: Scriptable Object responsible for the way dialogue text is revealed
    ///   <list type="table">
    ///       <item>
    ///           <description><b>audioPerCharacter</b> - The audio that plays after each and every character is revealed </description>
    ///       </item>
    ///       <item>
    ///           <description><b>revealIntervalMs</b> - The speed at which the dialogue text is revealed </description>
    ///       </item>
    ///   </list>
    /// </summary>
    [CreateAssetMenu]
    public class RevealStyle : ScriptableObject
    {
        public EventReference audioPerCharacter;
        // The speed at which the dialogue text is revealed
        public int revealIntervalMs;
    }
}

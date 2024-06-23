using FMODUnity;
using Plugins.FMOD.src;
using UnityEngine;

// Description: This class determines the way dialogue text is presented/revealed

namespace TextPresentation
{
    [CreateAssetMenu]
    public class RevealStyle : ScriptableObject
    {
        public EventReference audioPerCharacter;
        // The speed at which the dialogue text is revealed
        public int revealIntervalMs;
    }
}

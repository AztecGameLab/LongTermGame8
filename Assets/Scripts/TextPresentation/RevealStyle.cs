using FMODUnity;
using Plugins.FMOD.src;
using UnityEngine;

namespace TextPresentation
{
    [CreateAssetMenu]
    public class RevealStyle : ScriptableObject
    {
        public EventReference audioPerCharacter;
        public int revealIntervalMs;
    }
}

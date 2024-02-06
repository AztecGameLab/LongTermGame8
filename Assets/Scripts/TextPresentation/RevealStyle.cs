using FMODUnity;
using UnityEngine;
namespace Ltg8
{
    [CreateAssetMenu]
    public class RevealStyle : ScriptableObject
    {
        public EventReference audioPerCharacter;
        public int revealIntervalMs;
    }
}

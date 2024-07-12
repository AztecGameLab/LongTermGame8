using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Ltg8.Audio
{
    [CreateAssetMenu(fileName = "AudioReferences", menuName = "Audio/Audio References")]
    public class AudioReferences : ScriptableObject
    {
        [Header("Player Movement")] 
        public EventReference footsteps;
        public EventReference jump;
        public EventReference descent;

        [Header("Player Interactions")]
        // Rope climbing sfx
        public EventReference placeRope;
        public EventReference climbRope;
        
        // Boulder sfx
        public EventReference rolling;
        public EventReference crashing;
        
        // Cat-apult sfx
        public EventReference placePart;
        public EventReference loading;
        public EventReference rotating;
        public EventReference launch;
        public EventReference tune;

        [Header("Environment")] 
        public EventReference ambiance;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Ltg8.Audio
{
    public class WalkingAudio : MonoBehaviour
    {
        // TO DO:
        // --- ADD METHODS FOR DIFFERENT FEET
        // --- IMPORT AUDIO INTO FMOD STUDIO
        // --- ADD AUDIO TO ScriptObj Sounds/AudioReferences
        // --- FIX THE FOLLOWING:
        // ------ 'NewItemCollectedAnimation' AnimationEvent 'PlayWalkingSound' on animation 
        //        'Animations_Walk' has no receiver! Are you missing a component?
        
        public void PlayWalkingSound(AudioReferences playClip)
        {
            if (playClip.footsteps.IsNull) return;
                
            RuntimeManager.PlayOneShot(playClip.footsteps);
        }
    }
}

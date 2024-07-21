using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Ltg8.Audio
{
    [CreateAssetMenu(fileName = "AudioReferences", menuName = "Audio/Audio References")]
    public class AudioReferences : ScriptableObject
    {
        [Header("Environment")] 
        public EventReference ambiance;
        
        [Header("Jingles")]
        public EventReference friendBond;
        public EventReference puzzleCompletion;
        public EventReference stickerCollect;
        
        [Header("Ester Misc")]
        // Ester
        public EventReference detection;
        public EventReference rise;
        public EventReference warp;

        [Header("Music")]
        public EventReference bondingTheme;
        public EventReference caveMusic;
        public EventReference estersTheme;
        public EventReference openingCutscene;
        public EventReference rockyJumpRoad;
        
        [Header("Player Interactions")]
        // Rope climbing sfx
        public EventReference climbRope;
        public EventReference placeRope;
        
        // Boulder sfx
        public EventReference crashing;
        public EventReference rolling;
        
        // Cat-apult sfx
        public EventReference launch;
        public EventReference loading;
        public EventReference placePart;
        public EventReference rotating;
        public EventReference tune;
        
        /* DONE */
        [Header("Player Movement")]
        //public EventReference descent;
        public EventReference footsteps;
        public EventReference jump;
        public EventReference landing;
        
        [Header("UI")] 
        public EventReference select;
        
        // Dialogue
        public EventReference sigmund;
        public EventReference eggy;
        public EventReference ester;
        
        // Dialogue SFX
        public EventReference cameraShot;
        public EventReference dataUpload;
        public EventReference radioStatic;
        
        // Sticker UI
        public EventReference cameraSticker;
        public EventReference cameraPartSticker;
        public EventReference catapultPartSticker;
        public EventReference openStickerMenu;
        public EventReference ropeSticker;
        public EventReference stickerAppear;
    }
}


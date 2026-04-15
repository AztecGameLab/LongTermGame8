using System;
using UnityEngine;

namespace Animation.FlipBook
{
    
    /// <summary>
    /// <b>Description</b>: This class holds the data for the Sprite Flipbook Animation (character sprites used in dialogue), specifically the sprites
    /// it contains and the interval between sprite updates
    /// <list type="table">
    ///       <item> <description><b>sprites[]</b> - An empty array of sprites </description> </item>
    ///       <item> <description><b>updateRateSeconds</b> - A float that determines the interval between updates for Update() </description> </item>
    ///   </list>
    /// </summary>
    // NOTE: Kinda a lot to explain here, but in the top menu (File, Edit, Assets, . . .), under Assets->Create, this will create a menu for this class.
    [CreateAssetMenu]
    public class SpriteFlipBookData : ScriptableObject
    {
        // Creates an empty array of sprites
        public Sprite[] sprites = Array.Empty<Sprite>();
        
        // The interval between updates for Update()
        public float updateRateSeconds = 0.15f;
    }
    
    /// <summary>
    ///   <b>Description</b>: This (serializable) class controls the animation for Sprite FlipBooks, specifically the 
    /// sequencing and spacing of sprite alternations
    /// <list type="number">
    ///       <item> <description><b><see cref="ApplyTo"/></b></description> </item>
    ///       <item> <description><b><see cref="Update"/></b></description> </item>
    ///   </list>
    /// </summary>
    [Serializable]
    public class SpriteFlipBookAnimation : IFlipBookAnimation
    {
        /// <summary> <b>Description</b>: The sprite animation data: <see cref="SpriteFlipBookData"/> </summary>
        [SerializeField]
        private SpriteFlipBookData data;

        // The current index on the array of sprites 
        private int _index;
        
        // Sets _elapsed to a very large number (infinity)
        private float _elapsed = float.PositiveInfinity;

        /// <summary> <b>Description</b>: Makes the Flipbook object display the next sprite in the sequence, based on the index. </summary>
        /// <param name="view"> The Flipbook object that displays the image </param>
        public void ApplyTo(FlipBookView view)
        {
            // If not null data, display the sprite at _index of the sprite array. Otherwise, pass null.
            view.DisplayImage(data != null ? data.sprites[_index] : null);
        }
        
        /// <summary> <b>Description</b>: Update() w/ a custom interval. Responsible for incrementing (and looping) the sprite
        /// index, changing the sprite that is displayed. </summary>
        /// <param name="deltaTime"> A float that determines the interval between updates </param>
        public void Update(float deltaTime)
        {
            // Do not continue if data is null
            if (data == null) return;
            
            // If _elapsed is greater than the specified interval
            if (_elapsed > data.updateRateSeconds)
            {
                // Increment the index (starting over if the end of the array is reached)
                _index = (_index + 1) % data.sprites.Length;
                // Set the time elapsed back to 0
                _elapsed = 0;
            }
            // Increment the time elapsed every update, by the increment given in the parameters
            _elapsed += deltaTime;
        }
    }
}

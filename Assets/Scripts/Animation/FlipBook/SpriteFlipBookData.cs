using System;
using UnityEngine;

namespace Animation.FlipBook
{
    // NOTE: Kinda a lot to explain here, but in the top menu (File, Edit, Assets, . . .), under Assets->Create, this will create a menu for this class.
    [CreateAssetMenu]
    //NOTE: This class extends the ScriptableObject class
    
    /* ----------------------------------------------------------------------------------------------------
        Description
        This class holds the data for the Sprite Flipbook (used in dialogue), specifically the sprites it contains and the interval between sprite updates
       ----------------------------------------------------------------------------------------------------
    */
    
    public class SpriteFlipBookData : ScriptableObject
    {
        // Creates an empty array of sprites
        public Sprite[] sprites = Array.Empty<Sprite>();
        
        // The interval between updates for Update()
        public float updateRateSeconds = 0.15f;
    }
    
    /* ----------------------------------------------------------------------------------------------------
        Description
        This class controls the animation for the Sprite FlipBook, specifically the sequencing and spacing of
        sprite alternations
       ----------------------------------------------------------------------------------------------------
    */
    
    [Serializable]
    // NOTE: This class implements IFlipBookAnimation
    public class SpriteFlipBookAnimation : IFlipBookAnimation
    {
        // Declares an instance of the SpriteFlipBookData class
        [SerializeField]
        private SpriteFlipBookData data;

        // The index of interest on the array of sprites 
        private int _index;
        
        // Sets _elapsed to a very large number (infinity)
        private float _elapsed = float.PositiveInfinity;

        // Displays the next sprite in the sequence, based on the index
        public void ApplyTo(FlipBookView view)
        {
            // If the data is not null, display the sprite at _index of the array of sprites. Otherwise, pass null.
            view.DisplayImage(data != null ? data.sprites[_index] : null);
        }
        
        // Called in an Update event, basically update with a specified interval and a focus on time elapsed
        public void Update(float deltaTime)
        {
            // Do not continue if data is null
            if (data == null)
                return;
            
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

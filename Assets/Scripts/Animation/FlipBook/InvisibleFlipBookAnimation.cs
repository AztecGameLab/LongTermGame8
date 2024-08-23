using UnityEngine;

namespace Animation.FlipBook
{
    
    /// <summary>
    ///   <b>Description</b>: This class simply creates a 1x1 transparent sprite for the Flipbook Object to display.
    /// <list type="number">
    ///       <item>
    ///           <description><b><see cref="ApplyTo"/></b></description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class InvisibleFlipBookAnimation : IFlipBookAnimation
    {
        // NOTE: Readonly implies that the variable can only be instantiated in the constructor and never again
        // NOTE: Static, the variable is accessible amongst all instances of the class
        // The code below creates an Instance of InvisibleFlipBookAnimation using the constructor. This instance is global and unchangeable
        public static readonly InvisibleFlipBookAnimation Instance = new InvisibleFlipBookAnimation();
        private readonly Sprite _transparentTexture;
            
        // Constructor for this class, creates the transparent texture
        private InvisibleFlipBookAnimation()
        {
            // Creates a new 2D texture with a size of 1 x 1 (1 pixel)
            var texture = new Texture2D(1, 1);
            // Sets the (only) pixel, determined by the first two parameters, to the color set in the third parameter
            texture.SetPixel(0, 0, Color.clear);
            // Changes made on the CPU are copied to the GPU (some hardware thing I guess)
            texture.Apply();
            // Creates a new 1 x 1 sprite at the center of the screen, using the texture made in this constructor
            _transparentTexture = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0, 0));
        }

        /// <summary>
        /// <b>Description</b>: Makes the Flipbook object display a transparent sprite.
        /// </summary>
        /// <param name="view"> The Flipbook object that displays the image </param>
        public void ApplyTo(FlipBookView view)
        {
            // Displays the image using the "_transparentTexture" Sprite created in this class
            view.DisplayImage(_transparentTexture);
        }
        
        public void Update(float deltaTime)
        {
            /* Do nothing. */
        }
    }
}

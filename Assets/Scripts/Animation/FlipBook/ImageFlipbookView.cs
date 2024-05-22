using UnityEngine;
using UnityEngine.UI;

namespace Animation.FlipBook
{
    // NOTE: This class provides implementations for the methods in FlipBookView.cs
    
    /* ---------------------------------------------------------------------
        Description
        This class seems to control the display of FlipBook Sprites
       ---------------------------------------------------------------------
    */
    
    public class ImageFlipbookView : FlipBookView
    {
        [SerializeField] private Image image; // The image modified by the class

        // Displays "Image" with the sprite provided in the parameters
        public override void DisplayImage(Sprite sprite)
        {
            // NOTE: The ? operator is the conditional operator, basically a fancier if-else statement
            // If the sprite is null, set the color of "image" to clear, otherwise, set the color of "image" to white
            image.color = sprite == null ? Color.clear : Color.white;
            // Set the sprite of "Image" to the sprite given in the parameters
            image.sprite = sprite;
        }
    }
}

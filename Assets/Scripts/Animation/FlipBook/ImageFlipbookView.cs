using UnityEngine;
using UnityEngine.UI;

namespace Animation.FlipBook
{
    // NOTE: This class provides implementations for the methods in FlipBookView.cs
    
    /// <summary>
    ///   <b>Description</b>: Controls the display of Flipbook (Dialogue) sprites
    /// <list type="table">
    ///       <item>
    ///           <description><b>DisplayImage()</b> - Displays an image, utilizing the sprite provided in the parameters </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class ImageFlipbookView : FlipBookView
    {
        [SerializeField] private Image image; // The image modified by the class

        /// <summary>
        ///   <b>Description</b>: Displays the "Image" with the sprite provided in the parameters
        /// </summary>
        /// <param name="sprite"> The displayed Sprite</param>
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

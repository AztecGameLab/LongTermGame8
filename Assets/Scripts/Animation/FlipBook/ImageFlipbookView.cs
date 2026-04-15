using UnityEngine;
using UnityEngine.UI;

namespace Animation.FlipBook
{
    /// <summary>
    ///   <b>Description</b>: This class contains the Flipbook (Character) sprite that appears during dialogue, and is responsible for the 
    /// sprite image it displays.
    /// <list type="bullet">
    ///       <item>
    ///           <description> <b><see cref="DisplayImage"/></b> </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class ImageFlipbookView : FlipBookView
    {
        [SerializeField] private Image image; // The image modified by the class

        /// <summary>
        ///   <b>Description</b>: Changes the game object's "image" to the sprite provided in the parameters
        /// </summary> <param name="sprite"> The sprite image to display </param>
        public override void DisplayImage(Sprite sprite)
        {
            // If null sprite, set "image" color to clear (invisible); else, set "image" color to white
            image.color = sprite == null ? Color.clear : Color.white;
            // Set the sprite of "image" to the sprite given in the parameters
            image.sprite = sprite;
        }
    }
}

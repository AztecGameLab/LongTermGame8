using UnityEngine;

namespace Animation.FlipBook
{
    // NOTE: Abstract (Base) Class, the implementation is defined in the derived classes
     
    // NOTE: I believe this is a Abstract Class as opposed to an Interface (unlike IFlipBookAnimation.cs) because it needs to extend MonoBehavior
    
    /// <summary>
    ///   <b>Description</b>: Abstract Class for displaying Flipbook sprites
    /// <list type="table">
    ///       <item>
    ///           <description><b>DisplayImage()</b> - Displays an image, utilizing the sprite provided in the parameters </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public abstract class FlipBookView : MonoBehaviour
    {
        /// <summary>
        ///   <b>Description</b>: Displays an image
        /// </summary>
        /// <param name="sprite"> The displayed Sprite</param>
        public abstract void DisplayImage(Sprite sprite);
    }
}

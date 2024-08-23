using UnityEngine;

namespace Animation.FlipBook
{
    
    /// <summary>
    ///   <b>Description</b>: Abstract Class for displaying FlipBook sprites
    /// <list type="number">
    ///       <item>
    ///           <description> <b><see cref="ImageFlipbookView"/></b> </description>
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

using UnityEngine;

namespace Animation.FlipBook
{
    
    /// <summary>
    /// <b>Description</b>: Abstract Class for displaying FlipBook sprites
    /// <list type="number">
    ///       <item> <description> <b><see cref="ImageFlipbookView"/></b> </description> </item>
    ///   </list>
    /// </summary>
    public abstract class FlipBookView : MonoBehaviour
    {
        /// <summary>
        ///   <b>Description</b>: Changes the image an object will display
        /// </summary>
        /// <param name="sprite"> The sprite image to display</param>
        public abstract void DisplayImage(Sprite sprite);
    }
}

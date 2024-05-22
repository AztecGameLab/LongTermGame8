using UnityEngine;

namespace Animation.FlipBook
{
    /* Abstract (Base) Class, the implementation is defined in the derived classes
     *
     * Derived Classes --> ImageFlipbooxView.cs 
     * 
     */
    // NOTE: I believe this is a Abstract Class as opposed to an Interface (unlike IFlipBookAnimation.cs) because it needs to extend MonoBehavior
    public abstract class FlipBookView : MonoBehaviour
    {
        // NOTE: Abstract Method, implementation is defined in the derived classes
        // Displays an image, utilizing the sprite provided in the parameters
        public abstract void DisplayImage(Sprite sprite);
    }
}

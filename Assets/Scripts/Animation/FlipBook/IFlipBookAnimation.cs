namespace Animation.FlipBook
{
    /// <summary>
    ///   <b>Description</b>: This is an interface for any/all Flipbook Animation classes
    /// <list type="number">
    ///       <item>
    ///           <description><b><see cref="SpriteFlipBookAnimation"/></b> </description>
    ///       </item>
    ///       <item>
    ///           <description><b> <see cref="InvisibleFlipBookAnimation"/> </b> </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IFlipBookAnimation
    {
        /// <summary>
        ///   <b>Description</b>: Makes the Flipbook object display the next sprite in the sequence, based on the index
        /// </summary>
        /// <param name="view"> An object of FlipBookView or one of its subclasses </param>
        void ApplyTo(FlipBookView view);
        /// <summary>
        ///   <b>Description</b>: Update with a custom interval, increments the sprite index. 
        /// </summary>
        /// <param name="deltaTime"> A float that determines the interval between updates </param>
        void Update(float deltaTime);
    }
}

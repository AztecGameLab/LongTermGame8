namespace Animation.FlipBook
{
    // NOTE: This is an interface, no implementations
    
    /// <summary>
    ///   <b>Description</b>: This is an interface for any/all Flipbook Animation classes
    /// <list type="table">
    ///       <item>
    ///           <description><b>ApplyTo()</b> - Transitions Flipbook to next frame/sprite </description>
    ///       </item>
    ///       <item>
    ///           <description><b>Update()</b> - Controls the intervals between sprite transitions </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IFlipBookAnimation
    {
        // NOTE: These are abstract methods, implementation must be done in derived classes
        /// <summary>
        ///   <b>Description</b>: Displays the next sprite in the sequence, based on the index
        /// </summary>
        /// <param name="view"> An object of FlipBookView or one of its subclasses </param>
        void ApplyTo(FlipBookView view);
        /// <summary>
        ///   <b>Description</b>: Basically update with a specified interval and a focus on time elapsed
        /// </summary>
        /// <param name="deltaTime"> A float that determines the interval between updates </param>
        void Update(float deltaTime);
    }
}

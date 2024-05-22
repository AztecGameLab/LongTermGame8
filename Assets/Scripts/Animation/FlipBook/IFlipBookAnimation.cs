namespace Animation.FlipBook
{
    // NOTE: This is an interface, no implementations
    /* Interface, the implementation is defined in the derived classes
     *
     * Implemented Classes --> InvisibleFlipBookAnimation.cs, SpriteFlipBookAnimation.cs
     *
     */
    public interface IFlipBookAnimation
    {
        // NOTE: These are abstract methods, implemenetation must be done in derived classes
        void ApplyTo(FlipBookView view);
        void Update(float deltaTime);
    }
}

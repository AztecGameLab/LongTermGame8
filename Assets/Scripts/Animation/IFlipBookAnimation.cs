namespace Ltg8
{
    public interface IFlipBookAnimation
    {
        void ApplyTo(FlipBookView view);
        void Update(float deltaTime);
    }
}

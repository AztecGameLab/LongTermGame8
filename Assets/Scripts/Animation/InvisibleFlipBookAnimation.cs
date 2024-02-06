using UnityEngine;
namespace Ltg8
{
    public class InvisibleFlipBookAnimation : IFlipBookAnimation
    {
        public static readonly InvisibleFlipBookAnimation Instance = new InvisibleFlipBookAnimation();
        private readonly Texture2D _transparentTexture;

        public InvisibleFlipBookAnimation()
        {
            _transparentTexture = new Texture2D(1, 1);
            _transparentTexture.SetPixel(0, 0, Color.clear);
            _transparentTexture.Apply();
        }

        public void ApplyTo(FlipBookView view)
        {
            view.DisplayImage(_transparentTexture);
        }
        
        public void Update(float deltaTime)
        {
            /* Do nothing. */
        }
    }
}

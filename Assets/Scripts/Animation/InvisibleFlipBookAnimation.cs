using UnityEngine;
namespace Ltg8
{
    public class InvisibleFlipBookAnimation : IFlipBookAnimation
    {
        public static readonly InvisibleFlipBookAnimation Instance = new InvisibleFlipBookAnimation();
        private readonly Sprite _transparentTexture;

        public InvisibleFlipBookAnimation()
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.clear);
            texture.Apply();
            
            _transparentTexture = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0, 0));
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

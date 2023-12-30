using UnityEngine;
using UnityEngine.UI;
namespace Ltg8
{
    public class ImageFlipBookView : FlipBookView
    {
        [SerializeField] 
        private Image image;
        
        public override void DisplayImage(Texture2D texture)
        {
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Ltg8
{
    public class ImageFlipbookView : FlipBookView
    {
        [SerializeField] private Image image;

        public override void DisplayImage(Sprite sprite)
        {
            image.color = sprite == null ? Color.clear : Color.white;
            image.sprite = sprite;
        }
    }
}

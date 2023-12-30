using UnityEngine;
using UnityEngine.UI;

namespace Ltg8
{
    public class RawImageFlipBookView : FlipBookView
    {
        [SerializeField] 
        private RawImage image;
        
        public override void DisplayImage(Texture2D texture)
        {
            image.texture = texture; 
        }
    }
}

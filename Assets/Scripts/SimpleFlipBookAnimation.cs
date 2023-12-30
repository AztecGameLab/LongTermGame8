using System;
using UnityEngine;
namespace Ltg8
{
    [Serializable]
    public class SimpleFlipBookAnimation : IFlipBookAnimation
    {
        public Texture2D[] textures;
        public float updateRateSeconds = 0.5f;

        private int _index;
        private float _elapsed = float.PositiveInfinity;
        
        public void UpdateOn(FlipBookView view, float deltaTime)
        {
            if (_elapsed > updateRateSeconds)
            {
                view.DisplayImage(textures[_index]);
                _index = (_index + 1) % textures.Length;
                _elapsed = 0;
            }
            
            _elapsed += deltaTime;
        }
    }
}

using System;
using UnityEngine;

namespace Ltg8
{
    [CreateAssetMenu]
    public class SimpleFlipBookData : ScriptableObject
    {
        public Texture2D[] textures = Array.Empty<Texture2D>();
        public float updateRateSeconds = 0.15f;
    }
    
    [Serializable]
    public class SimpleFlipBookAnimation : IFlipBookAnimation
    {
        [SerializeField]
        private SimpleFlipBookData data;

        private int _index;
        private float _elapsed = float.PositiveInfinity;

        public void ApplyTo(FlipBookView view)
        {
            view.DisplayImage(data.textures[_index]);
        }
        
        public void Update(float deltaTime)
        {
            if (_elapsed > data.updateRateSeconds)
            {
                _index = (_index + 1) % data.textures.Length;
                _elapsed = 0;
            }
            
            _elapsed += deltaTime;
        }
    }
}

﻿using System;
using UnityEngine;

namespace Ltg8
{
    [CreateAssetMenu]
    public class SpriteFlipBookData : ScriptableObject
    {
        public Sprite[] sprites = Array.Empty<Sprite>();
        public float updateRateSeconds = 0.15f;
    }
    
    [Serializable]
    public class SpriteFlipBookAnimation : IFlipBookAnimation
    {
        [SerializeField]
        private SpriteFlipBookData data;

        private int _index;
        private float _elapsed = float.PositiveInfinity;

        public void ApplyTo(FlipBookView view)
        {
            if (data != null)
                view.DisplayImage(data.sprites[_index]);
            else view.DisplayImage(null);
        }
        
        public void Update(float deltaTime)
        {
            if (data == null)
                return;
            
            if (_elapsed > data.updateRateSeconds)
            {
                _index = (_index + 1) % data.sprites.Length;
                _elapsed = 0;
            }
            
            _elapsed += deltaTime;
        }
    }
}

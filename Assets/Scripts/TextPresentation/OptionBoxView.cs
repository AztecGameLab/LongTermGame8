using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TNRD;
using UnityEngine;
using UnityEngine.Pool;

namespace Ltg8
{
    public abstract class OptionBoxView : MonoBehaviour
    {
        [SerializeField] private TextBoxView textBox;
        
        public ImageFlipbookView optionAnimationImage;
        public EventReference optionHoverSfx;
        public EventReference optionSelectSfx;
        
        [SerializeField]
        private SingleOptionView singleOptionPrefab;

        protected ObjectPool<SingleOptionView> OptionPool;
        protected IFlipBookAnimation Animation;
        public TextBoxView TextBox => textBox;

        protected virtual void Awake()
        {
            InvisibleFlipBookAnimation.Instance.ApplyTo(optionAnimationImage);
            OptionPool = new ObjectPool<SingleOptionView>(() => Instantiate(singleOptionPrefab));
        }

        private void Update()
        {
            Animation?.ApplyTo(optionAnimationImage);
        }

        public abstract UniTask<int> PickOption(OptionData first, OptionData second);
        public abstract UniTask<int> PickOption(OptionData first, OptionData second, OptionData third);
        public abstract UniTask<int> PickOption(OptionData first, OptionData second, OptionData third, OptionData fourth);
    }
    
    [Serializable]
    public struct OptionData
    {
        [SerializeField]
        private SerializableInterface<IFlipBookAnimation> animation;
        
        public string message;
        public IFlipBookAnimation Animation => animation.Value;

        public static implicit operator OptionData(string s)
        {
            return new OptionData 
            {
                message = s,
                animation = new SerializableInterface<IFlipBookAnimation>(InvisibleFlipBookAnimation.Instance),
            };
        }
    }
}

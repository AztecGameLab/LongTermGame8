using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Ltg8
{
    public abstract class OptionBoxView : MonoBehaviour
    {
        public TMP_Text mainText;
        public RawImageFlipBookView mainAnimationImage;
        public GameObject mainAnimationObject;
        public RawImageFlipBookView optionAnimationImage;
        public GameObject optionAnimationObject;
        public EventReference optionHoverSfx;
        public EventReference optionSelectSfx;

        public GameObject nameObject;
        public TMP_Text nameText;
        
        [SerializeField]
        private SingleOptionView singleOptionPrefab;

        protected ObjectPool<SingleOptionView> OptionPool;

        public virtual void Initialize()
        {
            mainText.SetText(string.Empty);
            nameText.SetText(string.Empty);
            
            nameObject.SetActive(false);
            optionAnimationObject.SetActive(false);
            mainAnimationObject.SetActive(false);
            
            OptionPool = new ObjectPool<SingleOptionView>(() => Instantiate(singleOptionPrefab));
        }

        public abstract UniTask<int> OptionPickTwo(string first, string second);
        public abstract UniTask<int> OptionPickThree(string first, string second, string third);
        public abstract UniTask<int> OptionPickFour(string first, string second, string third, string fourth);
    }
}

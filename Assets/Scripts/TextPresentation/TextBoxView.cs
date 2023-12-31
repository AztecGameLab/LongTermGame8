using TMPro;
using TriInspector;
using UnityEngine;

namespace Ltg8
{
    public class TextBoxView : MonoBehaviour
    {
        [Title("Main Text")]
        public TMP_Text mainText;
        public GameObject mainAnimationObject;
        public RawImageFlipBookView mainAnimationImage;
        public GameObject continueHint;

        [Title("Name Box")]
        public GameObject nameObject;
        public TMP_Text nameBoxText;

        public void Initialize()
        {
            nameBoxText.SetText(string.Empty);
            mainText.SetText(string.Empty);
            
            nameObject.SetActive(false);
            mainAnimationObject.SetActive(false);
            continueHint.SetActive(false);
            
            mainText.maxVisibleCharacters = 0;
        }
    }
}

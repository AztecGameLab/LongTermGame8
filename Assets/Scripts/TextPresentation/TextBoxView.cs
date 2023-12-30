using TMPro;
using TriInspector;
using UnityEngine;

namespace Ltg8
{
    public class TextBoxView : MonoBehaviour
    {
        [Title("Main Text")]
        public GameObject normalFrame;
        public TMP_Text normalText;
        public RawImageFlipBookView normalAnimationImage;
        public GameObject continueHint;
        
        [Title("Options")]
        public GameObject optionFrame;
        public GameObject optionParent;
        public OptionView optionPrefab;
        public GameObject optionSelectionHint;
        
        [Title("Name Box")]
        public GameObject nameBoxFrame;
        public TMP_Text nameBoxText;

        public void Initialize()
        {
            normalFrame.SetActive(true);
            optionFrame.SetActive(false);
            nameBoxFrame.SetActive(false);
            continueHint.SetActive(false);
            optionParent.SetActive(false);
            normalAnimationImage.gameObject.SetActive(false);
            nameBoxText.text = string.Empty;
            normalText.text = string.Empty;
            normalText.maxVisibleCharacters = 0;
        }
    }
}

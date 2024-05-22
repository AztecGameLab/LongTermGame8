using System;
using TextPresentation;
using UnityEngine;

// todo: more randomization on the chirps (ref celeste, talk w/ luke)
// todo: radial option box cleanup (needs audio, more control over different choice amounts - diff. view for each option amount?)
// todo: better test w/ fake convo
// todo: better authoring of options (better callbacks instead of switch)

namespace Ltg8
{
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [SerializeField] private TextBoxView defaultTextBox;
        public TextBoxView DefaultTextBox => defaultTextBox;
        
        [SerializeField] private OptionBoxView defaultOptionsBox;
        public OptionBoxView DefaultOptionBox => defaultOptionsBox;

        [SerializeField] private TextBoxView eggyTextBox;
        public TextBoxView EggyTextBox => eggyTextBox;
        
        [SerializeField] private TextBoxView sigmundTextBox;
        public TextBoxView SigmundTextBox => sigmundTextBox;
    }
}

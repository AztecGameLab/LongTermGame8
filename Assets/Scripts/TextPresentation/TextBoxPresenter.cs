using System;
using TextPresentation;
using UnityEngine;

// TODO: more randomization on the chirps (ref celeste, talk w/ luke)
// TODO: radial option box cleanup (needs audio, more control over different choice amounts - diff. view for each option amount?)
// TODO: better test w/ fake convo
// TODO: better authoring of options (better callbacks instead of switch)


namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: GameObject that holds the scripts for all TextBoxView scripts, along w/ the default OptionBoxView Script
    /// </summary>
    [Serializable]
    public class TextBoxPresenter : MonoBehaviour
    {
        [Tooltip("[TextBoxView] The default text box view script")]
        [SerializeField] private TextBoxView defaultTextBox;
        public TextBoxView DefaultTextBox => defaultTextBox;
        
        [Tooltip("[OptionBoxView] The default options box view script")]
        [SerializeField] private OptionBoxView defaultOptionsBox;
        public OptionBoxView DefaultOptionBox => defaultOptionsBox;

        [Tooltip("[TextBoxView] Eggy's text box view script")]
        [SerializeField] private TextBoxView eggyTextBox;
        public TextBoxView EggyTextBox => eggyTextBox;
        
        [Tooltip("[TextBoxView] Sigmund's text box view script")]
        [SerializeField] private TextBoxView sigmundTextBox;
        public TextBoxView SigmundTextBox => sigmundTextBox;
    }
}

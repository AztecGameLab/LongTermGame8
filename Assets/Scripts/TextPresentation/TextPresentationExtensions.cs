using Cysharp.Threading.Tasks;
namespace Ltg8
{
    public static class TextPresentationExtensions
    {
        public static void CopyStateTo(this TextBoxView self, TextBoxView other)
        {
            other.CurrentRevealStyle = self.CurrentRevealStyle;
            other.CurrentText = self.CurrentText;
            other.CurrentDisplayName = self.CurrentDisplayName;
            other.CurrentMainAnimation = self.CurrentMainAnimation;
        }
        
        public static async UniTask<int> PickOption(this TextBoxView textBox, OptionBoxView optionBox, OptionData first, OptionData second)
        {
            optionBox.gameObject.SetActive(true);
            textBox.gameObject.SetActive(false);
            
            textBox.CopyStateTo(optionBox.TextBox);
            int result = await optionBox.PickOption(first, second);
            
            optionBox.gameObject.SetActive(false);
            textBox.gameObject.SetActive(true);
            return result;
        }
        
        public static async UniTask<int> PickOption(this TextBoxView textBox, OptionBoxView optionBox, OptionData first, OptionData second, OptionData third)
        {
            optionBox.gameObject.SetActive(true);
            textBox.gameObject.SetActive(false);
            
            textBox.CopyStateTo(optionBox.TextBox);
            int result = await optionBox.PickOption(first, second, third);
            
            optionBox.gameObject.SetActive(false);
            textBox.gameObject.SetActive(true);
            return result;
        }
        
        public static async UniTask<int> PickOption(this TextBoxView textBox, OptionBoxView optionBox, OptionData first, OptionData second, OptionData third, OptionData fourth)
        {
            optionBox.gameObject.SetActive(true);
            textBox.gameObject.SetActive(false);
            
            textBox.CopyStateTo(optionBox.TextBox);
            int result = await optionBox.PickOption(first, second, third, fourth);
            
            optionBox.gameObject.SetActive(false);
            textBox.gameObject.SetActive(true);
            return result;
        }
        
    }
}

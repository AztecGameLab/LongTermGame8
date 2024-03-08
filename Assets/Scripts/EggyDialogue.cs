using System;
using Cysharp.Threading.Tasks;
using Ltg8.Inventory;

namespace Ltg8
{
    public class EggyDialogue : ItemTarget
    {
        public Dialogue[] dialogueText;
        private int _currentDialogue;
        
        public override bool CanReceiveItem(ItemData data)
        {
            return data.guid == "items/walkie_talkie";
        }

        public override void ReceiveItem(ItemData item)
        {
            base.ReceiveItem(item);
            RunDialogueTask().Forget();
        }

        private async UniTask RunDialogueTask()
        {
            TextBoxView tb = Ltg8.TextBoxPresenter.EggyTextBox;
            tb.gameObject.SetActive(true);
            tb.ResetAllState();
            tb.CurrentDisplayName = "Eggy";
            
            foreach (string line in dialogueText[_currentDialogue].textFrames)
            {
                await tb.ClearText();
                await tb.WriteText(line);
                await tb.WaitForContinue();
            }
            
            tb.gameObject.SetActive(false);
            _currentDialogue = (_currentDialogue + 1) % dialogueText.Length;
        }

        public override bool WillConsumeItem() => false;

        [Serializable] public class Dialogue
        {
            public string[] textFrames;
        }
    }
}

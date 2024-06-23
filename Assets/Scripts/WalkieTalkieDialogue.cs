using Cysharp.Threading.Tasks;
using Inventory;
using Ltg8.Inventory;

namespace Ltg8
{
    public class WalkieTalkieDialogue : ItemTarget
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
            await dialogueText[_currentDialogue].Run();
            _currentDialogue = (_currentDialogue + 1) % dialogueText.Length;
        }

        public override bool WillConsumeItem() => false;
    }
}

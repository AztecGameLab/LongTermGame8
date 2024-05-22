using Animation.FlipBook;
using Cysharp.Threading.Tasks;
using TextPresentation;
using UnityEngine;

namespace Ltg8
{
    public class TestingTextBoxConsumer : MonoBehaviour
    {
        public SpriteFlipBookAnimation animSmile;
        public SpriteFlipBookAnimation animFrown;

        private TextBoxView t => Ltg8.TextBoxPresenter.DefaultTextBox;
        private OptionBoxView o => Ltg8.TextBoxPresenter.DefaultOptionBox;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
                TestTextBoxes().Forget();
            
            animSmile.Update(Time.deltaTime);
            animFrown.Update(Time.deltaTime);
        }

        private async UniTaskVoid TestTextBoxes()
        {
            t.gameObject.SetActive(true);
            t.ResetAllState();
            t.CurrentDisplayName = "Narrator";

            // basic test
            await t.WriteText("Hello, world!");
            await t.WaitForContinue();
            
            // delay + multi-part test
            await t.ClearText();
            await t.WriteText("Here is another line of text ...");
            await t.Delay(1);
            await t.WriteText(" and we're done!");
            await t.WaitForContinue();

            // decision + animation test
            await t.ClearText();
            t.CurrentMainAnimation = animSmile;
            await t.WriteText("Wanna make a <color=green>choice</color>?");
            await t.WaitForContinue();

            switch (await t.PickOption(o, "Sure", "No, choices scare me."))
            {
                case 0:
                    await Option_LikesChoices();
                    break;
                case 1:
                    await t.WriteText("That's fair.");
                    break;
            }

            await t.WaitForContinue();
            t.gameObject.SetActive(false);
        }

        private async UniTask Option_LikesChoices()
        {
            await t.WriteText("Well, how do you feel about three choices?");
            await t.WaitForContinue();
            
            switch (await t.PickOption(o, "I can handle it.", "How am I supposed to handle three choices?", "How can the textbox system can support this?"))
            {
                case 0:
                    await Option_ReallyLikesChoices();
                    break;
                case 1:
                    await t.WriteText("I'm sorry, of course three is too much.");
                    break;
                case 2:
                    await t.WriteText("Honestly, I have no clue.");
                    break;
            }
        }

        private async UniTask Option_ReallyLikesChoices()
        {
            await t.WriteText("You'll love this, then!");
            await t.WaitForContinue();

            switch (await t.PickOption(o, "WTF", "FOUR OPTIONS?!?!", "Are you crazy?", "ez"))
            {
                case 0:
                    await t.WriteText("Yep.");
                    break;
                case 1:
                    await t.WriteText("Read em' and weep.");
                    break;
                case 2:
                    await t.WriteText("Only a little.");
                    break;
                case 3:
                    await t.WriteText("I'm sorry, but four is the highest we can go currently...");
                    break;
            }
        }
    }
}

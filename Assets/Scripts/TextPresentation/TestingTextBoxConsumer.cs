using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    public class TestingTextBoxConsumer : MonoBehaviour
    {
        public SimpleFlipBookAnimation animSmile;
        public SimpleFlipBookAnimation animFrown;

        private TextBoxPresenter p => Ltg8.TextBoxPresenter;
        
        private void Start()
        {
            TestTextBoxes().Forget();
        }

        private void Update()
        {
            animSmile.Update(Time.deltaTime);
            animFrown.Update(Time.deltaTime);
        }

        private async UniTaskVoid TestTextBoxes()
        {
            await p.Open("Narrator");

            // basic test
            await p.WriteText("Hello, world!");
            await p.WaitForContinue();
            
            // delay + multi-part test
            await p.ClearText();
            await p.WriteText("Here is another line of text ...");
            await p.Delay(1);
            await p.WriteText(" and we're done!");
            await p.WaitForContinue();

            // decision + animation test
            await p.ClearText();
            await p.ShowMainAnimation(animSmile);
            await p.WriteText("Wanna make a <color=green>choice</color>?");
            await p.WaitForContinue();
            await p.ShowOptionAnimation(animSmile);

            switch (await p.PickOption("Sure", "No, choices scare me."))
            {
                case 0:
                    await Option_LikesChoices();
                    break;
                case 1:
                    await p.WriteText("That's fair.");
                    break;
            }

            await p.WaitForContinue();
            await p.Close();
        }

        private async UniTask Option_LikesChoices()
        {
            await p.WriteText("Well, how do you feel about three choices?");
            await p.WaitForContinue();
            
            switch (await p.PickOption("I can handle it.", "How am I supposed to handle three choices?", "How can the textbox system can support this?"))
            {
                case 0:
                    await Option_ReallyLikesChoices();
                    break;
                case 1:
                    await p.WriteText("I'm sorry, of course three is too much.");
                    break;
                case 2:
                    await p.WriteText("Honestly, I have no clue.");
                    break;
            }
        }

        private async UniTask Option_ReallyLikesChoices()
        {
            await p.WriteText("You'll love this, then!");
            await p.WaitForContinue();

            switch (await p.PickOption("WTF", "FOUR OPTIONS?!?!", "Are you crazy?", "ez"))
            {
                case 0:
                    await p.WriteText("Yep.");
                    break;
                case 1:
                    await p.WriteText("Read em' and weep.");
                    break;
                case 2:
                    await p.WriteText("Only a little.");
                    break;
                case 3:
                    await p.WriteText("I'm sorry, but four is the highest we can go currently...");
                    break;
            }
        }
    }
}

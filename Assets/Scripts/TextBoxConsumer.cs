using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    public class TextBoxConsumer : MonoBehaviour
    {
        public SimpleFlipBookAnimation animSmile;
        public SimpleFlipBookAnimation animFrown;
        
        private void Start()
        {
            TestTextBoxes().Forget();
        }

        private async UniTaskVoid TestTextBoxes()
        {
            TextBoxPresenter p = Ltg8.TextBoxPresenter;

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
            await p.ShowAnimation(animSmile);
            await p.WriteText("Wanna make a <color=green>choice</color>?");
            await p.WaitForContinue();
            
            int result = await p.PrepareDecision()
                    .WithOption("Nah, not really.")
                    .WithOption("Sure!")
                    .WithOption("...")
                    .Present();
            switch (result) {
                case 0: 
                    await p.ShowAnimation(animFrown);
                    await p.WriteText("Wow, you are <color=red>lazy</color>."); 
                    break;
                case 1: 
                    await p.WriteText("I like your attitude!"); 
                    break;
                case 2: 
                    await p.ShowAnimation(animFrown);
                    await p.WriteText("You ignoring me?"); 
                    break;
            }
            await p.WaitForContinue();
            
            await p.Close();
        }
    }
}

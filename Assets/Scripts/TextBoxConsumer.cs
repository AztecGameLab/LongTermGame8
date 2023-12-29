using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    public class TextBoxConsumer : MonoBehaviour
    {
        private void Start()
        {
            TestTextBoxes().Forget();
        }

        private async UniTaskVoid TestTextBoxes()
        {
            TextBoxPresenter p = Ltg8.TextBoxPresenter;

            await p.Open();
            
            await p.WriteText("Hello, world!");
            await p.WaitForContinue();
            
            await p.ClearText();
            await p.WriteText("Here is another line of text ...");
            await UniTask.Delay(1000);
            await p.WriteText(" and we're done!");
            await p.WaitForContinue();
            
            await p.Close();
        }
    }
}

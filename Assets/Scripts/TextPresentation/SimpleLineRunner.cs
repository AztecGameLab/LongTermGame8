using Cysharp.Threading.Tasks;
using TextPresentation;
using UnityEngine;

namespace Ltg8
{
    public class SimpleLineRunner : MonoBehaviour
    {
        [SerializeField] private string text;

        public void Play()
        {
            InternalPlay().Forget();
        }

        private async UniTaskVoid InternalPlay()
        {
            TextBoxView textbox = Ltg8.TextBoxPresenter.DefaultTextBox;
            textbox.ResetAllState();
            textbox.gameObject.SetActive(true);
            await textbox.WriteText(text);
            await textbox.WaitForContinue();
            textbox.gameObject.SetActive(false);
        }
    }
}

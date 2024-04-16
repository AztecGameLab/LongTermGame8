using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    [CreateAssetMenu]
    public class Dialogue : ScriptableObject
    {
        [Serializable] public class Frame
        {
            public string text;
            public DialogueCharacter character;
        }

        public enum DialogueCharacter
        {
            Eggy, Sigmund, Default,
        }
            
        public Frame[] frames;
        public static bool _isRunning;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _isRunning = false;
        }

        public void RunAndForget()
        {
            Run().Forget();
        }

        public async UniTask Run()
        {
            if (_isRunning)
                return;
            
            foreach (Frame frame in frames)
            {
                // setup textbox based on character
                TextBoxView view;
                    
                switch (frame.character)
                {
                    case DialogueCharacter.Eggy:
                        view = Ltg8.TextBoxPresenter.EggyTextBox;
                        view.ResetAllState();
                        view.CurrentDisplayName = "Eggy";
                        break;
                    case DialogueCharacter.Sigmund:
                        view = Ltg8.TextBoxPresenter.SigmundTextBox;
                        view.ResetAllState();
                        view.CurrentDisplayName = "Sigmund";
                        break;
                    case DialogueCharacter.Default:
                        view = Ltg8.TextBoxPresenter.DefaultTextBox;
                        view.ResetAllState();
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
                    
                // run dialogue
                _isRunning = true;
                view.gameObject.SetActive(true);

                await view.ClearText();
                await view.WriteText(frame.text);
                await view.WaitForContinue();
                    
                view.gameObject.SetActive(false);
                _isRunning = false;
            }
        }
    }
}

using System;
using Cysharp.Threading.Tasks;
using poetools.PluginScripts.Executions;
using TextPresentation;
using UnityEngine;

namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: This class is responsible for storing the data for dialogue, including
    /// speakers and the actual dialogue text. It also contains methods for executing dialogue sequences.
    /// </summary>
    [CreateAssetMenu]
    public class Dialogue : ScriptableObject
    {
        /// <summary> Data class, holds the dialogue text and the characters speaking </summary>
        [Serializable] public class Frame
        {
            public string text;
            public DialogueCharacter character;
        }

        /// <summary> Enumeration for the character speaking </summary>
        public enum DialogueCharacter
        {
            Eggy, Sigmund, Default,
        }
            
        // The dialogue snippets held in this Dialogue Object
        public Frame[] frames; 
        public static bool _isRunning; // Whether dialogue is currently running or not
        public static bool _ignoreDialogue; // Whether to ignore dialogue (activated in console)

        // Sets the _isRunning boolean to false *real early* in the program's execution
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() { _isRunning = false; }

        /// <summary> Activates the "ignore dialogue" command </summary>
        /// <param name="set"> Boolean value </param>
        public static void SetIgnoreDialogue(bool set) { _ignoreDialogue = set; }

        // Run the UniTask method. Running a UniTask method either requires an "await" keyword or ".Forget()"
        public void RunAndForget() { Run().Forget(); }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async UniTask Run()
        {
            var player = GameObject.Find("Player");
            
            if (_isRunning || player.GetComponent<IgnoreDialogue>().DoesIgnoreDialogue())
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
                if(player.GetComponent<IgnoreDialogue>().DoesIgnoreDialogue())
                {
                    return;
                }
            }
        }
    }
}

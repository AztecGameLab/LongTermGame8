using UnityEngine;

namespace poetools.Console
{
    /// <summary>
    /// Old-input system controller for the console.
    /// </summary>
    public class InputConsoleController : MonoBehaviour
    {
        public KeyCode visibilityToggleKey = KeyCode.BackQuote;
        public KeyCode cycleAutoCompleteKey = KeyCode.Tab;
        public KeyCode historyBackwardKey = KeyCode.UpArrow;
        public KeyCode historyForwardKey = KeyCode.DownArrow;

        private void Update()
        {
            bool toggleVisibility = Input.GetKeyDown(visibilityToggleKey);
            bool cycleAutoComplete = Input.GetKeyDown(cycleAutoCompleteKey);
            bool historyBackward = Input.GetKeyDown(historyBackwardKey);
            bool historyForward = Input.GetKeyDown(historyForwardKey);

            foreach (RuntimeConsole console in FindObjectsOfType<RuntimeConsole>(true))
            {
                if (toggleVisibility)
                    console.ToggleVisibility();

                if (console.IsVisible())
                {
                    if (cycleAutoComplete)
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            console.CycleAutoCompleteBackward();
                        
                        else console.CycleAutoCompleteForward();
                    }

                    if (historyBackward)
                        console.MoveHistoryBackward();

                    if (historyForward)
                        console.MoveHistoryForward();
                }
            }
        }
    }
}

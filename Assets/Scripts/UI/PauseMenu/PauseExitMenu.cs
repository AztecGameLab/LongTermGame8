using UnityEngine;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class PauseExitMenu : MonoBehaviour
    {
        private const int NumSaveSlots = 6;
        
        // References
        private Button _buttonExitToStart;
        private Button _buttonExitToDesktop;

        // Start is called before the first frame update
        private void Start()
        {
            // Getting root to reach the other elements of UI document
            var root = GetComponent<UIDocument>().rootVisualElement;

            // Getting references for...
            _buttonExitToStart = root.Q<Button>("ButtonExitToStart");
            _buttonExitToDesktop = root.Q<Button>("ButtonExitToOS");

            // ...When pressing yes/no on the confirmation
            _buttonExitToStart.clicked += ExitStart;
            _buttonExitToDesktop.clicked += ExitOS;
        }

        private static async void ExitStart()
        {
            await Ltg8.GameState.TransitionTo(new MainMenuGameState());
        }

        private static void ExitOS()
        {
            Application.Quit();
        }
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
namespace Ltg8
{
    /// <summary>
    /// <b>Description</b>: Methods for entering and exiting the Main Menu Game State
    /// <list type="number">
    ///       <item> <description><b><see cref="OnEnter()"/></b></description> </item>
    ///       <item> <description><b><see cref="OnExit()"/></b></description> </item>
    /// </list>
    /// </summary>
    public class MainMenuGameState : IGameState
    {
        /// <summary>
        /// When entering the state, change to the Main Menu Scene.
        /// </summary>
        public async UniTask OnEnter()
        {
            string path = Ltg8.Settings.mainMenuScenePath; // The path to the Main Menu Scene
            // Load the Main Menu Scene without unloading the currently loaded scenes
            await SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            // Set the Main Menu Scene to the active scene
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
        }
        
        /// <summary>
        /// Method to run when exiting the state. Doesn't do anything at the moment
        /// </summary>
        public UniTask OnExit() { return UniTask.CompletedTask;}

        public void OnUpdate()
        {}
    }
}

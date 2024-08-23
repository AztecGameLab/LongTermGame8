using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: 
    /// <list type="number">
    ///       <item>
    ///           <description><b>___</b> </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class MainMenuGameState : IGameState
    {
        /// <summary>
        /// 
        /// </summary>
        public async UniTask OnEnter()
        {
            string path = Ltg8.Settings.mainMenuScenePath; // The path to the Main Menu Scene
            await SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
        }
        
        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
        }
    }
}

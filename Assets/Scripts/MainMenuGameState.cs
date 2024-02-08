using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
namespace Ltg8
{
    public class MainMenuGameState : IGameState
    {
        public async UniTask OnEnter()
        {
            string path = Ltg8.Settings.mainMenuScenePath;
            await SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
        }
        
        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Ltg8
{
    public class OverworldGameState : IGameStateLogic
    {
        private readonly string _scenePath;
        
        public OverworldGameState(string scenePath)
        {
            _scenePath = scenePath;
        }
        
        public async UniTask OnEnter()
        {
            await SceneManager.LoadSceneAsync(_scenePath, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_scenePath));
        }
        
        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Ltg8
{
    public class OverworldGameState : IGameState
    {
        private readonly string _scenePath;

        public AsyncStateMachine<IOverworldState> StateMachine { get; }
        public ExploringState ExploringState { get; }
        public InteractingState InteractingState { get; }
        public PausedState PausedState { get; }
        
        public OverworldGameState(string scenePath)
        {
            _scenePath = scenePath;
            
            ExploringState = new ExploringState {
                OverworldState = this,
            };
            
            InteractingState = new InteractingState {
                OverworldState = this,
            };

            PausedState = new PausedState {
                OverworldState = this,
            };
            
            StateMachine = new AsyncStateMachine<IOverworldState>();
        }
        
        public async UniTask OnEnter()
        {
            await SceneManager.LoadSceneAsync(_scenePath, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_scenePath));
            
            await StateMachine.TransitionTo(ExploringState); // todo: maybe some worlds transition and don't default to exploring...
        }
        
        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            StateMachine.CurrentState?.OnUpdate();
        }
    }
}

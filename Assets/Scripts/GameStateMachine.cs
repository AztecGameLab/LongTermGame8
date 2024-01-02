using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    public class GameStateMachine : MonoBehaviour
    {
        public IGameStateLogic CurrentState { get; private set; } = new NullGameState();
        public bool IsTransitioning { get; private set; }

        public async UniTask TransitionTo(IGameStateLogic state)
        {
            if (IsTransitioning)
            {
                Debug.LogWarning("Tried to transition while a transition is already occuring!");
                return;
            }
            
            IsTransitioning = true;
            await CurrentState.OnExit();
            CurrentState = state;
            await CurrentState.OnEnter();
            IsTransitioning = false;
        }
    }
    
    public interface IGameStateLogic
    {
        UniTask OnEnter();
        UniTask OnExit();
    }

    public class NullGameState : IGameStateLogic
    {
        public UniTask OnEnter()
        {
            // do nothing
            return UniTask.CompletedTask;
        }
        
        public UniTask OnExit()
        {
            // do nothing
            return UniTask.CompletedTask;
        }
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    public class AsyncStateMachine<T> where T : IAsyncState
    {
        public T CurrentState { get; private set; }
        public bool IsTransitioning { get; private set; }
        
        public async UniTask TransitionTo(T state)
        {
            if (IsTransitioning)
            {
                Debug.LogWarning("Trying to transition in the middle of a transition! Stop!");
                return;
            }
            
            IsTransitioning = true;
            if (CurrentState != null) await CurrentState.OnExit();
            CurrentState = state;
            if (CurrentState != null) await CurrentState.OnEnter();
            IsTransitioning = false;
        }
    }
    
    public interface IAsyncState
    {
        UniTask OnEnter();
        UniTask OnExit();
    }
}

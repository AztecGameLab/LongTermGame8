using Cysharp.Threading.Tasks;

namespace Ltg8
{
    public class AsyncStateMachine<T> where T : IAsyncState
    {
        public T CurrentState { get; private set; }
        
        public async UniTask TransitionTo(T state)
        {
            if (CurrentState != null) await CurrentState.OnExit();
            CurrentState = state;
            if (CurrentState != null) await CurrentState.OnEnter();
        }
    }
    
    public interface IAsyncState
    {
        UniTask OnEnter();
        UniTask OnExit();
    }
}

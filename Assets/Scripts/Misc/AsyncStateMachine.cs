using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    /// <summary>
    /// <b>Description</b>: Class responsible for transitioning between different game states. Takes an
    /// innstance of <b><see cref="IAsyncState"/></b>.
    /// <list type="table">
    ///       <item> <description><b><see cref="TransitionTo"/></b></description> </item>
    /// </list>
    /// </summary>
    public class AsyncStateMachine<T> where T : IAsyncState
    {
        // Creates Getter and Setter methods for an IAsyncState instance "CurrentState"
        public T CurrentState { get; private set; }
        
        // Creates Getter and Setter methods for the boolean "IsTransitioning" (whether the current state is changing)
        public bool IsTransitioning { get; private set; }
        
        /// <summary>
        ///   <b>Description</b>: Transitions from the current state to another
        /// </summary>
        /// <param name="state" >The AsyncState to transition to</param>
        public async UniTask TransitionTo(T state)
        {
            if (IsTransitioning) // If already currently transitioning
            {
                // Give a warning and return
                Debug.LogWarning("Trying to transition in the middle of a transition! Stop!");
                return;
            }
            
            IsTransitioning = true; 
            // Before the transition,  wait until CurrentState's onExit() code has been executed
            if (CurrentState != null) await CurrentState.OnExit();
            CurrentState = state;
            // After the transition, if the CurrentState isn't null, wait until CurrentState's onEnter() code has been executed
            if (CurrentState != null) await CurrentState.OnEnter();
            IsTransitioning = false;
        }
    }
    
    /// <summary>
    /// <b>Description</b>: An interface for Asynchronous State Machines
    /// <list type="number">
    ///       <item> <description><b><see cref="IGameState"/></b></description> </item>
    ///       <item> <description><b><see cref="IOverworldState"/></b></description> </item>
    /// </list>
    /// </summary>
    public interface IAsyncState
    {
        /// <summary> <b>Description</b>: Code to run when transitioning into the scene </summary>
        UniTask OnEnter();
        /// <summary> <b>Description</b>: Code to run when transitioning out of the scene </summary>
        UniTask OnExit();
    }
}

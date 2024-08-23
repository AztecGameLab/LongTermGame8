using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: Class responsible for transitioning between different states
    /// <list type="table">
    ///       <item>
    ///           <description><b><see cref="CurrentState"/></b></description>
    ///       </item>
    ///       <item>
    ///           <description><b><see cref="TransitionTo"/></b></description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class AsyncStateMachine<T> where T : IAsyncState
    {
        /// <summary>
        /// Creates Getter and Setter methods for an AsyncState instance "CurrentState"
        /// </summary>
        public T CurrentState { get; private set; }
        
        // Creates Getter and Setter methods for the boolean "IsTransitioning" (whether the current state is changing)
        public bool IsTransitioning { get; private set; }
        
        /// <summary>
        ///   <b>Description</b>: Transitions from the current state to another
        /// </summary>
        /// <param name="state" >The state to transition to</param>
        public async UniTask TransitionTo(T state)
        {
            if (IsTransitioning) // If already currently transitioning
            {
                // Give a warning and return
                Debug.LogWarning("Trying to transition in the middle of a transition! Stop!");
                return;
            }
            
            IsTransitioning = true; 
            // Before the transition, if the CurrentState isn't null, wait until CurrentState's onExit() code has been executed
            if (CurrentState != null) await CurrentState.OnExit();
            CurrentState = state;
            // After the transition, if the CurrentState isn't null, wait until CurrentState's onEnter() code has been executed
            if (CurrentState != null) await CurrentState.OnEnter();
            IsTransitioning = false;
        }
    }
    
    /// <summary>
    ///   <b>Description</b>: An interface for Asynchronous State Machines
    /// <list type="number">
    ///       <item>
    ///           <description><b><see cref="IGameState"/></b></description>
    ///       </item>
    ///       <item>
    ///           <description><b><see cref="IOverworldState"/></b></description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IAsyncState
    {
        // Code that runs upon entering the state
        UniTask OnEnter();
        // Code that runs upon exiting the state
        UniTask OnExit();
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ltg8
{
    /// <summary>
    ///   <b>Description</b>: Class responsible for transitioning between different states
    /// <list type="table">
    ///       <item>
    ///           <description><b>CurrentState</b> - Getter/Setter for the game's CurrentState </description>
    ///       </item>
    ///       <item>
    ///           <description><b>TransitionTo()</b> - Transitions to the specified state </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public class AsyncStateMachine<T> where T : IAsyncState
    {
        // Creates Getter and Setter methods for an AsyncState instance "CurrentState"
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
            // Before the transition, if the CurrentState isn't null, wait until the CurrentState has been exited
            if (CurrentState != null) await CurrentState.OnExit();
            CurrentState = state;
            // After the transition, if the CurrentState isn't null, wait until the CurrentState has been entered
            if (CurrentState != null) await CurrentState.OnEnter();
            IsTransitioning = false;
        }
    }
    
    /// <summary>
    ///   <b>Description</b>: An interface for Asynchronous State Machines
    /// <list type="table">
    ///       <item>
    ///           <description><b>OnEnter()</b> - Runs upon entering the state </description>
    ///       </item>
    ///       <item>
    ///           <description><b>OnExit()</b> - Runs upon exiting the state </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IAsyncState
    {
        UniTask OnEnter();
        UniTask OnExit();
    }
}

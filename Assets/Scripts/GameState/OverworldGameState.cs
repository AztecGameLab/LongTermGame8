using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Ltg8
{
    /// <summary>
    /// <b>Description</b>: Methods for entering and exiting the Overworld Game State
    /// <list type="number">
    ///       <item> <description><b><see cref="OnEnter()"/></b></description> </item>
    ///       <item> <description><b><see cref="OnExit()"/></b></description> </item>
    /// </list>
    /// </summary>
    public class OverworldGameState : IGameState
    {
        private readonly string _scenePath; // The path of the Overworld Scene

        // A State Machine specifically for the game states of the Overworld
        public AsyncStateMachine<IOverworldState> StateMachine { get; }
        // Getter for the Exploring Game State
        public ExploringState ExploringState { get; }
        // Getter for the Interacting Game State
        public InteractingState InteractingState { get; }
        
        // Constructor for the Overworld Game State
        public OverworldGameState(string scenePath)
        {
            _scenePath = scenePath; // Sets the path for the Overworld Scene
            
            ExploringState = new ExploringState {
                OverworldState = this, // Initialize the Exploring State, with this as the OverworldState object
            };
            
            InteractingState = new InteractingState {
                OverworldState = this, // Initialize the Interacting State, with this as the OverworldState object
            };
            
            StateMachine = new AsyncStateMachine<IOverworldState>(); // Initializes the Overworld State Machine
        }
        
        /// <summary>
        /// <b>Description</b>: When entering this state; load the Overworld Scene using the scene path  set the active scene to this
        /// Overworld Scene, and Transition to the sub-state Exploring State
        /// </summary>
        public async UniTask OnEnter()
        {
            await SceneManager.LoadSceneAsync(_scenePath, LoadSceneMode.Additive); // Load the overworld state without unloading other states
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_scenePath)); // Set overworld state to the active scene
            
            // The initial state is the Exploring State
            await StateMachine.TransitionTo(ExploringState); // todo: maybe some worlds transition and don't default to exploring... (Daniel)
        }
        
        /// <summary>
        /// <b>Description</b>: Method to run when exiting the state. Doesn't do anything currently. 
        /// </summary>
        public UniTask OnExit() { return UniTask.CompletedTask; }

        // Doesn't really do anything since the sub-states don't have an onUpdate method, but typically would
        // run the Update method of the current state
        public void OnUpdate() { StateMachine.CurrentState?.OnUpdate(); }
    }
}

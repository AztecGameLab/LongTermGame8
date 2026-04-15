namespace Ltg8
{
    /// <summary>
    /// <b>Description</b>: An interface for Overworld States 
    /// <list type="number">
    ///       <item> <description><b><see cref="ExploringState"/></b></description> </item>
    ///       <item> <description><b><see cref="InteractingState"/></b></description> </item>
    /// </list>
    /// </summary>
    public interface IOverworldState : IAsyncState
    {
        // Getters and Setters for the Overworld Game State
        OverworldGameState OverworldState { get; set; }
        
        /// <summary> <b>Description</b>: Code to run on every frame update </summary>
        void OnUpdate();
    }
}

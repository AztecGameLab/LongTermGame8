namespace Ltg8
{
    
    // NOTE: Doc Complete
    
    /// <summary>
    ///   <b>Description</b>: An interface for Game States (implements IAsyncState)
    /// <list type="table">
    ///       <item>
    ///           <description><b>OnUpdate()</b> - Runs every tick update </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IGameState : IAsyncState
    {
        void OnUpdate();
    }
}

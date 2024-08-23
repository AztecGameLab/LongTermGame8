namespace Ltg8
{
    
    // NOTE: Doc Complete
    
    /// <summary>
    ///   <b>Description</b>: An interface for Game States 
    /// <list type="number">
    ///       <item>
    ///           <description><b><see cref="MainMenuGameState"/></b></description>
    ///       </item>
    ///       <item>
    ///           <description><b><see cref="OverworldGameState"/></b></description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IGameState : IAsyncState
    {
        // Code that is ran every frame
        void OnUpdate();
    }
}

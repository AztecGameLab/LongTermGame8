namespace Ltg8
{
    
    /// <summary>
    /// <b>Description</b>: An interface for Game States 
    /// <list type="number">
    ///       <item> <description><b><see cref="MainMenuGameState"/></b></description> </item>
    ///       <item> <description><b><see cref="OverworldGameState"/></b></description> </item>
    /// </list>
    /// </summary>
    public interface IGameState : IAsyncState
    {
        /// <summary> <b>Description</b>: Code to run on every frame update </summary>
        void OnUpdate();
    }
}

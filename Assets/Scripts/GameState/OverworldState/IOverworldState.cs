namespace Ltg8
{
    public interface IOverworldState : IAsyncState
    {
        OverworldGameState OverworldState { get; set; }
        void OnUpdate();
    }
}

namespace Ltg8
{
    public interface IGameState : IAsyncState
    {
        void OnUpdate();
    }
}

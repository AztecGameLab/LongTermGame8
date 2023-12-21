namespace poetools.Console.Commands
{
    public interface IConsoleDebugInfo
    {
        string DebugName { get; }
        void DrawDebugInfo();
    }
}

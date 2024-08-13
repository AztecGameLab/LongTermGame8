namespace poetools.Console.Commands
{
    /// <summary>
    ///   <b>Description</b>: Interface responsible for classes that can be debugged
    /// <list type="table">
    ///       <item>
    ///           <description><b>DebugName</b> - A string getter for the name of the debugger object </description>
    ///       </item>
    ///       <item>
    ///           <description><b>DrawDebugInfo</b> - The method for drawing up and saving debug info </description>
    ///       </item>
    ///   </list>
    /// </summary>
    public interface IConsoleDebugInfo
    {
        string DebugName { get; }
        void DrawDebugInfo();
    }
}

using UnityEngine;
namespace poetools.Console.Commands
{
    public class ConsoleDebugManager : MonoBehaviour
    {
        private readonly TargetData[] _data = new TargetData[10];
        
        public void Register(IConsoleDebugInfo target)
        {
            for (int i = 0; i < _data.Length; ++i)
            {
                if (!_data[i].IsValid)
                {
                    _data[i].IsValid = true;
                    _data[i].Target = target;
                    _data[i].WindowRect = new Rect(20, 20, 120, 50);
                    return;
                }
            }
        }

        private void OnGUI()
        {
            for (int i = 0; i < _data.Length; ++i)
            {
                if (_data[i].IsValid)
                    _data[i].WindowRect = GUILayout.Window(i, _data[i].WindowRect, WindowFunction, _data[i].Target.DebugName);
            }
        }

        private void WindowFunction(int windowId)
        {
            if (GUILayout.Button("x", GUILayout.Width(20)))
                _data[windowId].IsValid = false;
                    
            else _data[windowId].Target.DrawDebugInfo();
            
            GUI.DragWindow();
        }

        private struct TargetData
        {
            public bool IsValid;
            public Rect WindowRect;
            public IConsoleDebugInfo Target;
        }
    }
}

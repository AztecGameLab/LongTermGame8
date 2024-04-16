using UnityEngine;

namespace poetools.Console
{
    public abstract class LogPrefix : ScriptableObject
    {
        public abstract string GenerateMessage(string category);
    }
}

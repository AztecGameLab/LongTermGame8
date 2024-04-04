using UnityEngine;

namespace poetools.Console
{
    public abstract class UserPrefix : ScriptableObject
    {
        public abstract string GenerateMessage(string input);
    }
}
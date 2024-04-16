using UnityEngine;

namespace poetools.PluginScripts.Executions
{
    public class IgnoreDialogue : MonoBehaviour
    {
        
        private GameObject _player;
        
        // Is the IgnoreDialogue option active or deactivated.
        private bool _ignoreDialogue;

        private void Start()
        {
            _player = gameObject;
        }

        // Activates or Deactivates the IgnoreDialogue option
        public void ToggleIgnoreDialogue()
        {
            _ignoreDialogue = !_ignoreDialogue;
        }

        // Returns whether the IgnoreDialogue option is active or deactivated.
        public bool DoesIgnoreDialogue()
        {
            return (_ignoreDialogue);
        }
    }
}

using UnityEngine;

namespace poetools.PluginScripts.Executions
{
    public class NoClipMovement : MonoBehaviour
    {

        [Header("Developer Inputted Values")]
        
        // The player-character GameObject
        private Transform _player;
        
        [Tooltip("The speed of no-clipping")]
        [SerializeField] private float power;
        
        // Determines whether no-clipping is active or not
        private bool _noclip;
        
        // Is the console open?
        private bool _console;

        private void Start()
        {
            _player = gameObject.transform;
        }

        // Sets whether the console is open or not
        public void SetConsole(bool set) { _console = set; }
        
        // Activates or Deactivates Noclip
        public void ToggleNoclip() { _noclip = !_noclip; }

        //Returns whether noclip is activated or not
        public bool GetNoclip()
        { return _noclip;}

        private void Update()
        {
            // If no-clipping is disabled, or the console is open, return
            if (!_noclip || _console) return; 
            
            // Determines if any movement keys are being pressed
            var forward = Input.GetKey(KeyCode.W); var backward = Input.GetKey(KeyCode.S);
            var left = Input.GetKey(KeyCode.A); var right = Input.GetKey(KeyCode.D);
            var up = Input.GetKey(KeyCode.Space); var down = Input.GetKey(KeyCode.LeftShift);
            if (forward)
            {
                // Move in forward direction
                var fdirection = _player.forward * power;
                _player.localPosition += fdirection;
            }
            if (backward)
            {
                // Move in backward direction (opposite of forward)
                var bdirection = _player.forward * -power;
                _player.localPosition += bdirection;
            }

            if (left)
            {
                // Move in left direction (opposite of right)
                var ldirection = _player.right * -power;
                _player.localPosition += ldirection;
            }

            if (right)
            {
                // Move in right direction 
                var rdirection = _player.right * power;
                _player.localPosition += rdirection;
            }

            if(up)
            {
                // Move in upwards direction
                var udirection = _player.up * power;
                _player.localPosition += udirection;
            }

            if (down)
            {
                // Move in downwards direction (opposite of up) 
                var ddirection = _player.up * -power;
                _player.localPosition += ddirection;
            }
        }
    }
}
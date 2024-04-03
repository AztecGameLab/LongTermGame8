using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

    public class NoClipMovement : MonoBehaviour
    {

        private Transform _player;
        private bool _noclip;
        private bool _console;
        private float _power;

        // Start is called before the first frame update
        private void Start()
        {
            _player = gameObject.transform;
            _power = 0.5f;
        }

        public void ToggleNoclip() { _noclip = !_noclip; }

        public void SetConsole(bool set)
        {
            _console = set;
        }

        public bool GetNoclip()
        { return _noclip;}

        private void Update()
        {
            if (!_noclip) return;
            if (_console) return;
            var forward = Input.GetKey(KeyCode.W);
            var backward = Input.GetKey(KeyCode.S);
            var left = Input.GetKey(KeyCode.A);
            var right = Input.GetKey(KeyCode.D);
            var up = Input.GetKey(KeyCode.Space);
            var down = Input.GetKey(KeyCode.LeftShift);
            if (forward)
            {
                var fdirection = _player.forward * _power;
                _player.localPosition += fdirection;
            }
            if (backward)
            {
                var bdirection = _player.forward * -_power;
                _player.localPosition += bdirection;
            }

            if (left)
            {
                var ldirection = _player.right * -_power;
                _player.localPosition += ldirection;
            }

            if (right)
            {
                var rdirection = _player.right * _power;
                _player.localPosition += rdirection;
            }

            if(up)
            {
                var udirection = _player.up * _power;
                _player.localPosition += udirection;
            }

            if (down)
            {
                var ddirection = _player.up * -_power;
                _player.localPosition += ddirection;
            }
        }
    }
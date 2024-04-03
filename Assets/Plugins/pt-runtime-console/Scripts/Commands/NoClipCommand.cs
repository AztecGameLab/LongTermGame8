using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/NoClip")]
    public class NoClipCommand : Command
    {
        public override string Name => "noclip";
        public override string Help => "Allows you to fly and phase through the map with ease.";
        
        private bool _noClipToggle;

        private GameObject _player;

        public override void Execute(string[] args, RuntimeConsole console)
        {
            _player = GameObject.Find("Player");
            Debug.Log(_player);
            if (_noClipToggle)
            {
                console.Log(Name, "noclip disabled");
                _noClipToggle = false;
                TogglePhysics();
            }
            else
            {
                console.Log(Name, "noclip enabled");
                _noClipToggle = true;
                TogglePhysics();
            }
        }

        private void TogglePhysics()
        {
            _player.GetComponent<Rigidbody>().useGravity = false;
            _player.GetComponent<NoClipMovement>().ToggleNoclip();
        }
    }
}

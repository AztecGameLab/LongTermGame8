using poetools.Core.Abstraction;
using UnityEngine;

namespace pt_player_3d.Scripts.Movement
{
    [RequireComponent(typeof(PhysicsComponent))]
    public class PhysicsComponentDebugView : MonoBehaviour
    {
        private PhysicsComponent _physics;

        private void Start()
        {
            _physics = GetComponent<PhysicsComponent>();
        }

        private void OnGUI()
        {
            GUILayout.Label($"Velocity: {_physics.Velocity}");
            GUILayout.Label($"Speed: {_physics.Speed}");
            GUILayout.Label($"Running Speed: {_physics.RunningSpeed}");
        }
    }
}

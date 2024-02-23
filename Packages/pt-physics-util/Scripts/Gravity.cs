using pt_player_3d.Scripts;
using UnityEngine;

namespace poetools.Core.Abstraction
{
    [RequireComponent(typeof(IPhysicsComponent))]
    public class Gravity : MonoBehaviour
    {
        public Vector3 downDirection = Vector3.down;
        public float amount = -Physics.gravity.y;
        [SerializeField]private float idleGravity;
        [SerializeField] public GroundCheck3d groundCheck;

        private IPhysicsComponent _physicsComponent;

        private void Awake()
        {
            _physicsComponent = GetComponent<IPhysicsComponent>();
            // debug = DebugWhiteboard.Instance.AddLabel(() =>
                // $"velocity: {_physicsComponent.Velocity}\ngrounded: {_physicsComponent.IsGrounded}");
        }

        // private IDisposable debug;

        // private void OnDestroy()
        // {
            // debug.Dispose();
        // }

        private void FixedUpdate()
        {
            _physicsComponent.Velocity = !groundCheck.IsGrounded || _physicsComponent.Velocity.y > 0
                ? _physicsComponent.Velocity + downDirection * (amount * Time.deltaTime)
                : _physicsComponent.Velocity + Vector3.down * idleGravity;
        }
    }
}

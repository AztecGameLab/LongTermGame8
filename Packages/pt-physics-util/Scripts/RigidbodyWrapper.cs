using UnityEngine;

namespace poetools.Core.Abstraction.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyWrapper : PhysicsComponent
    {
        private Rigidbody _rigidbody;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }
    }
}
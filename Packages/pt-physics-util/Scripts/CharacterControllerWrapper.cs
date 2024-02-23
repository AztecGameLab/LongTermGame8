using poetools.Core.Abstraction;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerWrapper : PhysicsComponent
    {
        public bool autoUpdate = true;
        public bool autoSyncPhysics = true;

        private CharacterController _character;
        public override Vector3 Velocity { get; set; }

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (autoUpdate)
                Tick(Time.deltaTime, autoSyncPhysics);
        }

        /// <summary>
        /// Runs a simulation step for this character controller.
        /// </summary>
        /// <param name="deltaTime">The amount of time that the simulation should be incremented by. A negative value defaults to Time.deltaTime.</param>
        /// <param name="shouldSyncPhysics">If true, Physics.SyncTransforms is called after each step.</param>
        public void Tick(float deltaTime = -1, bool shouldSyncPhysics = true)
        {
            if (deltaTime <= 0)
                deltaTime = Time.deltaTime;

            // The built-in velocity function breaks for some reason, so we have to calc our own velocity.
            Vector3 beforePos = _character.transform.position;
            _character.Move(Velocity * deltaTime);
            Vector3 afterPos = _character.transform.position;
            Vector3 resultantVelocity = (afterPos - beforePos) / deltaTime;

            // This line ensures we can only lose speed from collisions. We never want to gain speed from a collision.
            if (Mathf.Round(resultantVelocity.sqrMagnitude) < Mathf.Round(Velocity.sqrMagnitude))
                Velocity = resultantVelocity;

            if (shouldSyncPhysics)
                Physics.SyncTransforms();
        }
    }
}

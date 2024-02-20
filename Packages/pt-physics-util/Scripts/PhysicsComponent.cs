using UnityEngine;

namespace poetools.Core.Abstraction
{
    public interface IPhysicsComponent
    {
        Vector3 Velocity { get; set; }
    }

    public abstract class PhysicsComponent : MonoBehaviour, IPhysicsComponent
    {
        public abstract Vector3 Velocity { get; set; }

        public float Speed => Velocity.magnitude;

        /// <summary>
        /// Gets a value indicating how fast this object is moving along the XZ plane.
        /// Essentially, ignores gravity in the calculation.
        /// </summary>
        public float RunningSpeed
        {
            get
            {
                Vector3 v = Velocity;
                v.y = 0;
                return v.magnitude;
            }
        }
    }
}

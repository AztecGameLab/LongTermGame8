using UnityEngine;
using UnityEngine.Events;

namespace pt_player_3d.Scripts
{
    public abstract class GroundCheck3d : MonoBehaviour
    {
        public UnityEvent<OnGroundLandEvent> onGroundLand;
        public UnityEvent<OnGroundLeaveEvent> onGroundLeave;

        public abstract bool IsGrounded { get; }
        public abstract bool TryGetGround(out GroundData3d groundData3d);

        public struct OnGroundLandEvent
        {
            public GroundData3d GroundBeingLandedOn;
        }

        public struct OnGroundLeaveEvent
        {
            public GroundData3d GroundBeingLeft;
        }
    }

    public struct GroundData3d
    {
        public Vector3 Normal;
        public Vector3 Point;
        public Collider Surface;
    }
}

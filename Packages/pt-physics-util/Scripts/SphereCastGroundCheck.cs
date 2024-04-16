using pt_player_3d.Scripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class SphereCastGroundCheck : GroundCheck3d
    {
        [SerializeField] private int maxHits = 5;
        [SerializeField] private LayerMask ignoreLayers = 1 << 2; // The "Ignore Raycasts" layer.

        public bool autoUpdate = true;
        public float radius = 1;
        public Vector3 center = Vector3.zero;
        public float slopeLimit = 45;
        public float maxDistance = 0.1f;

        private GroundData3d _data3d;
        private bool _wasGrounded;
        private bool _isGrounded;
        private RaycastHit[] _hits;

        private void Awake()
        {
            _hits = new RaycastHit[maxHits];
        }

        public override bool IsGrounded => _isGrounded;

        public override bool TryGetGround(out GroundData3d groundData3d)
        {
            groundData3d = _data3d;
            return _isGrounded;
        }

        private void Update()
        {
            if (autoUpdate)
                Tick();
        }

        /// <summary>
        /// Updates the internal state of the ground check.
        /// Caches the current grounded state for the frame and fires events if necessary.
        /// </summary>
        public void Tick()
        {
            GroundData3d currentGround = default;
            int hits = Physics.SphereCastNonAlloc(transform.position + center, radius, Vector3.down, _hits, maxDistance, ~ignoreLayers.value, QueryTriggerInteraction.Ignore);
            int validHits = 0;

            for (int i = 0; i < hits; i++)
            {
                RaycastHit currentHit = _hits[i];

                if (currentHit.collider.gameObject != gameObject && Vector3.Angle(currentHit.normal, transform.up) < slopeLimit)
                {
                    currentGround.Normal += currentHit.normal;
                    currentGround.Point += currentHit.point;
                    currentGround.Surface = currentHit.collider;
                    validHits++;
                }
            }

            // Average up all contacts.
            currentGround.Normal /= validHits;
            currentGround.Point /= validHits;
            _isGrounded = validHits > 0;

            if (_wasGrounded && !_isGrounded) // Just left
            {
                onGroundLeave.Invoke(new OnGroundLeaveEvent{GroundBeingLeft = _data3d});
            }

            if (!_wasGrounded && _isGrounded) // Just entered.
            {
                onGroundLand.Invoke(new OnGroundLandEvent{GroundBeingLandedOn = currentGround});
            }

            _data3d = currentGround;
            _wasGrounded = _isGrounded;
        }
    }
}

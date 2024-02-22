using System.Collections.Generic;
using UnityEngine;

namespace pt_player_3d.Scripts
{
    /// <summary>
    /// The highest-possible quality ground checking for physics-based GameObjects,
    /// which iterates over every current contact each physics update to ensure a
    /// perfectly up-to-date state.
    /// </summary>
    public class CollisionStay3dGroundCheck : GroundCheck3d
    {
        [SerializeField]
        [Tooltip("The steepest angle this object can remain grounded on.")]
        private float maxGroundAngle = 45;

        [SerializeField]
        [Tooltip("The transform that is used to judge the \"down\" direction.")]
        private Transform bodyTransform;

        private GroundData3d _data3d;
        private bool _isGrounded;
        private bool _wasGrounded;
        private List<ContactPoint> _frameContacts;
        private List<ContactPoint> _contactBuffer;

        public override bool IsGrounded => _isGrounded;

        public override bool TryGetGround(out GroundData3d groundData3d)
        {
            groundData3d = _data3d;
            return _isGrounded;
        }

        private void Start()
        {
            _frameContacts = new List<ContactPoint>();
            _contactBuffer = new List<ContactPoint>();
            _data3d = new GroundData3d();

            if (TryGetComponent(out Rigidbody rb))
                rb.sleepThreshold = 0;
        }

        private void OnValidate()
        {
            // negative ground angles are nonsensical, and so are values that would repeat
            maxGroundAngle = Mathf.Clamp(maxGroundAngle, 0, 180);
        }

        private void OnCollisionStay(Collision other)
        {
            int count = other.GetContacts(_contactBuffer);

            for (int i = 0; i < count; i++)
            {
                ContactPoint contact = _contactBuffer[i];

                if (Vector3.Angle(contact.normal, bodyTransform.up) < maxGroundAngle)
                    _frameContacts.Add(contact);
            }
        }

        private void FixedUpdate() // this runs after all "collisionStay" messages are processed.
        {
            _isGrounded = _frameContacts.Count > 0;

            if (!_isGrounded && _wasGrounded) // we just left
            {
                onGroundLeave.Invoke(new OnGroundLeaveEvent{GroundBeingLeft = _data3d});
            }

            // Compute the average normal and point based on all contacts.
            _data3d.Normal = Vector3.zero;
            _data3d.Point = Vector3.zero;

            foreach (ContactPoint contact in _frameContacts)
            {
                _data3d.Normal += contact.normal;
                _data3d.Point += contact.point;
                _data3d.Surface = contact.otherCollider;
            }

            _data3d.Normal /= _frameContacts.Count;
            _data3d.Point /= _frameContacts.Count;

            if (_isGrounded && !_wasGrounded) // we just landed
            {
                onGroundLand.Invoke(new OnGroundLandEvent{GroundBeingLandedOn = _data3d});
            }

            // prepare for the next frame's new collision data.
            _frameContacts.Clear();
            _wasGrounded = _isGrounded;
        }
    }
}

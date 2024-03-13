using UnityEngine;

namespace Ltg8.Player
{
    [CreateAssetMenu]
    public class MovementSettings : ScriptableObject
    {
        public float speed = 5;
        public float sprintSpeed = 8;
        public float normalFov = 60;
        public float sprintFov = 70;
        public float fovSpeed;
        public float groundAcceleration;
        public float groundDeceleration;
        public float airAcceleration;
        public float jumpSpeed = 5;
    }
}

using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerGroundCheck : SphereCastGroundCheck
    {
        [SerializeField]
        private CharacterController characterController;

        private void OnValidate()
        {
            if (characterController != null)
            {
                center = (characterController.height - 3 * characterController.radius) * Vector3.down + characterController.center;
                radius = characterController.radius;
                maxDistance = characterController.skinWidth + 0.01f;
                slopeLimit = characterController.slopeLimit;
            }
        }
    }
}

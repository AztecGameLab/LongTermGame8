using System;
using UnityEngine;
namespace DefaultNamespace
{
    public class FreeCam : MonoBehaviour
    {
        public float speed;
        private Vector3 _velocity;
        private Vector3 _rotation;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            // Update position
            Vector3 inputDir = new Vector3(
                GetInputAxis(KeyCode.D, KeyCode.A),
                GetInputAxis(KeyCode.Space, KeyCode.LeftShift),
                GetInputAxis(KeyCode.W, KeyCode.S)
            ).normalized;

            inputDir = transform.localToWorldMatrix.MultiplyVector(inputDir);
            _velocity = Vector3.Lerp(_velocity, inputDir * speed, 15*Time.deltaTime);
            transform.position += _velocity * Time.deltaTime;
            
            // Update rotation
            _rotation.x -= Input.GetAxisRaw("Mouse Y");
            _rotation.y += Input.GetAxisRaw("Mouse X");
            transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        }

        private static float GetInputAxis(KeyCode pos, KeyCode neg)
        {
            float result = 0;
            if (Input.GetKey(pos)) result++;
            if (Input.GetKey(neg)) result--;
            return result;
        }
    }
}

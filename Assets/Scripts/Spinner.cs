using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace DefaultNamespace
{
    public class Spinner : MonoBehaviour
    {
        public float spinSpeed;
        public Vector3 axis;

        private void Update()
        {
            transform.Rotate(axis, spinSpeed * Time.deltaTime);
        }
    }
}

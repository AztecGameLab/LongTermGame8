using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CurvatureTest : MonoBehaviour
    {
        private static readonly int Ltg8Curvature = Shader.PropertyToID("LTG8_CURVATURE");
        
        [Range(0, 0.05f)]
        public float curvature;

        private void OnValidate()
        {
            Shader.SetGlobalFloat(Ltg8Curvature, curvature);
        }

        private void Awake()
        {
            Shader.SetGlobalFloat(Ltg8Curvature, curvature);
        }
    }
}

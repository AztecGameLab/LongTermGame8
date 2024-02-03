using UnityEngine;

namespace Ltg8.Graphics
{
    public class CurvatureInitializer : MonoBehaviour
    {
        private static readonly int Ltg8Curvature = Shader.PropertyToID("LTG8_CURVATURE");
        
        [SerializeField] 
        [Range(0, 0.03f)]
        private float curvature = 0.015f;
 
        private void Start()
        {
            Shader.SetGlobalFloat(Ltg8Curvature, curvature);
        }
    }
}

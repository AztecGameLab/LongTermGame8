using UnityEngine;

namespace Ltg8.Graphics
{
    public class CameraColorSetter : MonoBehaviour
    {
        public Color color;
        
        private void Start()
        {
            Camera.main.backgroundColor = color;
        }
    }
}

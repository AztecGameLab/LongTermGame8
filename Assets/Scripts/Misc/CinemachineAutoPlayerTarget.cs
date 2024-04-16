using Cinemachine;
using Ltg8.Player;
using Player;
using UnityEngine;

namespace Misc
{
    public class CinemachineAutoPlayerTarget : MonoBehaviour
    {
        public CinemachineVirtualCamera cam;

        private void Start()
        {
            cam.Follow = FindAnyObjectByType<PlayerController>().transform;
        }
    }
}

using UnityEngine;
namespace Ltg8
{
    public class RadialOptionView : MonoBehaviour
    {
        public SingleOptionView optionView;
        public GameObject selectionHint;

        private void Awake()
        {
            selectionHint.SetActive(false);
        }
    }
}

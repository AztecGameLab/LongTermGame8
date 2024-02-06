using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ltg8
{
    public class SingleOptionView : MonoBehaviour
    {
        public TMP_Text textDisplay;
        public Button button;
        public UnityEvent onSelect;
        public UnityEvent onHover;

        private void OnEnable()
        {
            button.enabled = true;
        }

        private void OnDisable()
        {
            button.enabled = false;
        }

        public void HandleSelect()
        {
            onSelect?.Invoke();
        }

        public void HandleHover()
        {
            if (EventSystem.current.currentSelectedGameObject != button.gameObject)
            {
                button.Select();
                onHover?.Invoke();
            }
        }
    }
}

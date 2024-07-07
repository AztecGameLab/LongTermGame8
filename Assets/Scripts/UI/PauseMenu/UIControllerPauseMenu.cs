using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class UIControllerPauseMenu : MonoBehaviour
    {
        [SerializeField]
        private bool pauseMenuOnScreen = false;

        [SerializeField] private string currentPI = null;

        public UnityEvent onClose;

        private VisualElement _pauseMenu;
        private VisualElement _pageIndicator;

        private enum Display
        {
            Save,
            Load,
            Controls,
            Settings,
            Extra,
            Exit
        }

        private readonly Dictionary<Display, VisualElement> _menuElements = new();

        private Display _currDisplay = Display.Save;

        // Start is called before the first frame update
        private void Start()
        {
            // Getting root to reach the other elements of UI document
            var root = GetComponent<UIDocument>().rootVisualElement;

            // Getting references for...

            // Pause Menu and Page Indicator...
            _pauseMenu = root.Q<VisualElement>("PauseMenu");
            _pageIndicator = root.Q<VisualElement>("PageIndicator");

            // Different Menus
            foreach (Display display in Enum.GetValues(typeof(Display)))
            {
                Button button = root.Q<Button>("Button" + display);
                button.clicked += () => PageIndicatorToTab(display);
                _menuElements[display] = root.Q<VisualElement>(display + "Menu");
            }
        }

        public void Open()
        {
            if (!pauseMenuOnScreen)
            {
                Time.timeScale = 0;
                _pauseMenu.AddToClassList("PauseMenuOnScreen");
                pauseMenuOnScreen = true;
            }
        }

        public void Close()
        {
            if (pauseMenuOnScreen)
            {
                Time.timeScale = 1;
                _pauseMenu.RemoveFromClassList("PauseMenuOnScreen");
                pauseMenuOnScreen = false;

                onClose.Invoke();
            }
        }

        private void PageIndicatorToTab(Display tab)
        {
            // if the PageIndicator is already somewhere, remove the class from it
            if (currentPI != null)
            {
                _pageIndicator.RemoveFromClassList(currentPI);
            }

            // Make the currentPI = the new PI
            currentPI = "PageIndicatorAt" + tab;

            // Move PageIndicator
            _pageIndicator.AddToClassList(currentPI);

            // Turn off the previous display
            TurnOffCurrentDisplay();
            // Make Contacts the new current display
            _currDisplay = tab;

            // Turn on new display
            _menuElements[tab].style.display = DisplayStyle.Flex;
        }

        private void TurnOffCurrentDisplay()
        {
            _menuElements[_currDisplay].style.display = DisplayStyle.None;
        }
    }
}
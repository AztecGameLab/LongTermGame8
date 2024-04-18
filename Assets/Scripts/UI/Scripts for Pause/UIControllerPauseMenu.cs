using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class UIControllerPauseMenu : MonoBehaviour
    {
        [SerializeField] private bool PauseMenuOnScreen = false;
        [SerializeField] private string currentPI = null;

        public UnityEvent onClose;

        private VisualElement _PauseMenu;
        private VisualElement _PageIndicator;

        private Button _ButtonContacts;
        private Button _ButtonMap;
        private Button _ButtonCollections;
        private Button _ButtonQuestLog;
        private Button _ButtonSettings;
        private Button _ButtonSave;
        private Button _ButtonExit;

        public enum Display
        {
            Contacts,
            Map,
            Collection,
            Log,
            Settings,
            Save,
            Exit
        }

        private Dictionary<Display, VisualElement> menuElements = new();

        private Display currDisplay;

        // Start is called before the first frame update
        private void Start()
        {
            // Getting root to reach the other elements of UI document
            var root = GetComponent<UIDocument>().rootVisualElement;

            // Getting references for...

            // Pause Menu and Page Indicator...
            _PauseMenu = root.Q<VisualElement>("PauseMenu");
            _PageIndicator = root.Q<VisualElement>("PageIndicator");

            // Buttons...
            _ButtonContacts = root.Q<Button>("ButtonContacts");
            _ButtonMap = root.Q<Button>("ButtonMap");
            _ButtonCollections = root.Q<Button>("ButtonCollection");
            _ButtonQuestLog = root.Q<Button>("ButtonQuestLog");
            _ButtonSettings = root.Q<Button>("ButtonSettings");
            _ButtonSave = root.Q<Button>("ButtonSave");
            _ButtonExit = root.Q<Button>("ButtonExit");

            // and sub-menus
            menuElements[Display.Contacts] = root.Q<VisualElement>("ContactsMenu");
            menuElements[Display.Map] = root.Q<VisualElement>("MapMenu");
            menuElements[Display.Collection] = root.Q<VisualElement>("CollectionMenu");
            menuElements[Display.Log] = root.Q<VisualElement>("LogMenu");
            menuElements[Display.Settings] = root.Q<VisualElement>("SettingsMenu");
            menuElements[Display.Save] = root.Q<VisualElement>("SaveMenu");
            menuElements[Display.Exit] = root.Q<VisualElement>("ExitMenu");

            // Call methods to move the PageIndicator depending on which button is clicked
            _ButtonContacts.clicked += () => PageIndicatorToTab(Display.Contacts);
            _ButtonMap.clicked += () => PageIndicatorToTab(Display.Map);
            _ButtonCollections.clicked += () => PageIndicatorToTab(Display.Collection);
            _ButtonQuestLog.clicked += () => PageIndicatorToTab(Display.Log);
            _ButtonSettings.clicked += () => PageIndicatorToTab(Display.Settings);
            _ButtonSave.clicked += () => PageIndicatorToTab(Display.Save);
            _ButtonExit.clicked += () => PageIndicatorToTab(Display.Exit);
        }

        public void Open()
        {
            if (!PauseMenuOnScreen)
            {
                Time.timeScale = 0;
                _PauseMenu.AddToClassList("PauseMenuOnScreen");
                PauseMenuOnScreen = true;
            }
        }

        public void Close()
        {
            if (PauseMenuOnScreen)
            {
                Time.timeScale = 1;
                _PauseMenu.RemoveFromClassList("PauseMenuOnScreen");
                PauseMenuOnScreen = false;

                onClose.Invoke();
            }
        }

        private void PageIndicatorToTab(Display tab)
        {
            // if the PageIndicator is already somewhere, remove the class from it
            if (currentPI != null)
            {
                _PageIndicator.RemoveFromClassList(currentPI);
            }

            // Make the currentPI = the new PI
            currentPI = "PageIndicatorAt" + tab;

            // Move PageIndicator
            _PageIndicator.AddToClassList(currentPI);

            // Turn off the previous display
            TurnOffCurrentDisplay();
            // Make Contacts the new current display
            currDisplay = tab;

            // Turn on new display
            menuElements[tab].style.display = DisplayStyle.Flex;
        }

        private void TurnOffCurrentDisplay()
        {
            menuElements[currDisplay].style.display = DisplayStyle.None;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class UIControllerPauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode PauseKey = KeyCode.P;
    [SerializeField] private bool PauseMenuOnScreen = false;
    [SerializeField] private string currentPI = null;
    
    private VisualElement _PauseMenu;
    private VisualElement _PageIndicator;
    
    private Button _ButtonContacts;
    private Button _ButtonMap;
    private Button _ButtonCollections;
    private Button _ButtonQuestLog;
    private Button _ButtonSettings;
    private Button _ButtonSave;
    private Button _ButtonExit;

    private VisualElement _ContactsMenu;
    private VisualElement _MapMenu;
    private VisualElement _CollectionMenu;
    private VisualElement _LogMenu;
    private VisualElement _SettingsMenu;
    private VisualElement _SaveMenu;
    private VisualElement _ExitMenu;
    
    public enum CurrentDisplay
    {
        Contacts,
        Map,
        Collection,
        Log,
        Settings,
        Save,
        Exit
    }

    private CurrentDisplay currDisplay;
    // Start is called before the first frame update
    void Start()
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
        _ContactsMenu = root.Q<VisualElement>("ContactsMenu");
        _MapMenu = root.Q<VisualElement>("MapMenu");
        _CollectionMenu = root.Q<VisualElement>("CollectionMenu");
        _LogMenu = root.Q<VisualElement>("LogMenu");
        _SettingsMenu = root.Q<VisualElement>("SettingsMenu");
        _SaveMenu = root.Q<VisualElement>("SaveMenu");
        _ExitMenu = root.Q<VisualElement>("ExitMenu");
        
        // Call methods to move the PageIndicator depending on which button is clicked
        _ButtonContacts.RegisterCallback<ClickEvent>(PageIndicatorToContacts);
        _ButtonMap.RegisterCallback<ClickEvent>(PageIndicatorToMap);
        _ButtonCollections.RegisterCallback<ClickEvent>(PageIndicatorToCollection);
        _ButtonQuestLog.RegisterCallback<ClickEvent>(PageIndicatorToQuestLog);
        _ButtonSettings.RegisterCallback<ClickEvent>(PageIndicatorToSettings);
        _ButtonSave.RegisterCallback<ClickEvent>(PageIndicatorToSave);
        _ButtonExit.RegisterCallback<ClickEvent>(PageIndicatorToExit);

    }

    void Update()
    {
        if (Input.GetKeyDown(PauseKey))
        {
            if (PauseMenuOnScreen)
            {
                // If menu is on screen, take it out
                _PauseMenu.RemoveFromClassList("PauseMenuOnScreen");
                PauseMenuOnScreen = false;
            }
            else
            {
                // If menu isn't on screen, pull up Pause Menu
                _PauseMenu.AddToClassList("PauseMenuOnScreen");
                PauseMenuOnScreen = true;
            }
        }
    }

    private void PageIndicatorToContacts(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }
        
        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtContacts";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtContacts");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Contacts;
        
        // Turn on new display
        _ContactsMenu.style.display = DisplayStyle.Flex;
    }
    private void PageIndicatorToMap(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }
        
        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtMap";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtMap");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Map;
        
        // Turn on new display
        _MapMenu.style.display = DisplayStyle.Flex;
    }
    private void PageIndicatorToCollection(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }
        
        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtCollection";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtCollection");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Collection;
        
        // Turn on new display
        _CollectionMenu.style.display = DisplayStyle.Flex;
    }
    private void PageIndicatorToQuestLog(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }
        
        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtLog";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtLog");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Log;
        
        // Turn on new display
        _LogMenu.style.display = DisplayStyle.Flex;
    }
    private void PageIndicatorToSettings(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }
        
        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtSettings";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtSettings");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Settings;
        
        // Turn on new display
        _SettingsMenu.style.display = DisplayStyle.Flex;
    }
    private void PageIndicatorToSave(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }
        
        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtSave";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtSave");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Save;
        
        // Turn on new display
        _SaveMenu.style.display = DisplayStyle.Flex;
    }
    private void PageIndicatorToExit(ClickEvent evt)
    {
        // if the PageIndicator is already somewhere, remove the class from it
        if (currentPI != null)
        {
            _PageIndicator.RemoveFromClassList(currentPI);
        }

        // Make the currentPI = the new PI
        currentPI = "PageIndicatorAtExit";
        
        // Move PageIndicator
        _PageIndicator.AddToClassList("PageIndicatorAtExit");
        
        // Turn off the previous display
        TurnOffCurrentDisplay();
        // Make Contacts the new current display
        currDisplay = CurrentDisplay.Exit;
        
        // Turn on new display
        _ExitMenu.style.display = DisplayStyle.Flex;
    }
    

    private void TurnOffCurrentDisplay()
    {
        // Turn off the current displayed menu
        switch (currDisplay)
        {
            case CurrentDisplay.Contacts:
                Debug.Log("Turn off Contacts menu");
                _ContactsMenu.style.display = DisplayStyle.None;
                break;
            case CurrentDisplay.Map:
                Debug.Log("Turn off Map menu");
                _MapMenu.style.display = DisplayStyle.None;
                break;
            case CurrentDisplay.Collection:
                Debug.Log("Turn off Collection menu");
                _CollectionMenu.style.display = DisplayStyle.None;
                break;
            case CurrentDisplay.Log:
                Debug.Log("Turn off Log menu");
                _LogMenu.style.display = DisplayStyle.None;
                break;
            case CurrentDisplay.Settings:
                Debug.Log("Turn off Settings menu");
                _SettingsMenu.style.display = DisplayStyle.None;
                break;
            case CurrentDisplay.Save:
                Debug.Log("Turn off Save menu");
                _SaveMenu.style.display = DisplayStyle.None;
                break;
            case CurrentDisplay.Exit:
                Debug.Log("Turn off Exit menu");
                _ExitMenu.style.display = DisplayStyle.None;
                break;
            default:
                // No menu is being displayed
                Debug.Log("No current menu on display");
                break;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControllerPauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode PauseKey = KeyCode.P;
    [SerializeField] private bool PauseMenuOnScreen = false;
    [SerializeField] private string currentPI = null;
    [SerializeField] private string currentDisplay;
    
    private VisualElement _PauseMenu;
    private VisualElement _PageIndicator;
    
    private Button _ButtonContacts;
    private Button _ButtonMap;
    private Button _ButtonCollections;
    private Button _ButtonQuestLog;
    private Button _ButtonSettings;
    private Button _ButtonSave;
    private Button _ButtonExit;
    
    // Start is called before the first frame update
    void Start()
    {
        // Getting root to reach the other elements of UI document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Getting references
        _PauseMenu = root.Q<VisualElement>("PauseMenu");
        _PageIndicator = root.Q<VisualElement>("PageIndicator");
        
        _ButtonContacts = root.Q<Button>("ButtonContacts");
        _ButtonMap = root.Q<Button>("ButtonMap");
        _ButtonCollections = root.Q<Button>("ButtonCollection");
        _ButtonQuestLog = root.Q<Button>("ButtonQuestLog");
        _ButtonSettings = root.Q<Button>("ButtonSettings");
        _ButtonSave = root.Q<Button>("ButtonSave");
        _ButtonExit = root.Q<Button>("ButtonExit");
        
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
        
        
        // Turn on display for thing clicked
        // Turn off the previous display
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
        
        // Turn on display for thing clicked
        // Turn off the previous display
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
        
        // Turn on display for thing clicked
        // Turn off the previous display
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
        
        // Turn on display for thing clicked
        // Turn off the previous display
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
        
        // Turn on display for thing clicked
        // Turn off the previous display
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
        
        // Turn on display for thing clicked
        // Turn off the previous display
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
        
        // Turn on display for thing clicked
        // Turn off the previous display
    }
}

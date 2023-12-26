using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControllerPauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode PauseKey = KeyCode.P;
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
        _ButtonCollections = root.Q<Button>("ButtonCollections");
        _ButtonQuestLog = root.Q<Button>("ButtonQuestLog");
        _ButtonSettings = root.Q<Button>("ButtonSettings");
        _ButtonSave = root.Q<Button>("ButtonSave");
        _ButtonExit = root.Q<Button>("ButtonExit");
        
        // Call methods to move the PageIndicator depending on which button is clicked
        _ButtonContacts.RegisterCallback<ClickEvent>(PageIndicatorToContacts);
    }

    void Update()
    {
        if (Input.GetKeyDown(PauseKey))
        {
            // Pull up Pause Menu (Prob make a class the slides up the menu to the screen)
        }
    }

    private void PageIndicatorToContacts(ClickEvent evt)
    {
        // Move PageIndicator
        // Turn on display for thing clicked
        // Turn off the previous display
    }
}

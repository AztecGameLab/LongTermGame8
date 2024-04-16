using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class pauseSaveMenu : MonoBehaviour
{
    // References
    private Button _SaveSlot0;
    private Button _SaveSlot1;
    private Button _SaveSlot2;
    private Button _SaveSlot3;
    private Button _SaveSlot4;
    private Button _SaveSlot5;
    
    private VisualElement _SaveConfirmation;

    private Button _ButtonYesSaveConfirmation;
    private Button _ButtonNoSaveConfirmation;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Getting root to reach the other elements of UI document
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Getting references for...
        
        // Save Slots...
        _SaveSlot0 = root.Q<Button>("SaveSlot0");
        _SaveSlot1 = root.Q<Button>("SaveSlot1");
        _SaveSlot2 = root.Q<Button>("SaveSlot2");
        _SaveSlot3 = root.Q<Button>("SaveSlot3");
        _SaveSlot4 = root.Q<Button>("SaveSlot4");
        _SaveSlot5 = root.Q<Button>("SaveSlot5");
        
        // Save confirmation window
        _SaveConfirmation = root.Q<VisualElement>("SaveConfirmation");
        
        // Yes and no buttons for save confirmation...
        _ButtonYesSaveConfirmation = root.Q<Button>("ButtonYes");
        _ButtonNoSaveConfirmation = root.Q<Button>("ButtonNo");
        
        
        // Call methods...
        // ...when pressing on the save slots on the save menu
        _SaveSlot0.RegisterCallback<ClickEvent>(DisplaySaveConfirmation);
        _SaveSlot1.RegisterCallback<ClickEvent>(DisplaySaveConfirmation);
        _SaveSlot2.RegisterCallback<ClickEvent>(DisplaySaveConfirmation);
        _SaveSlot3.RegisterCallback<ClickEvent>(DisplaySaveConfirmation);
        _SaveSlot4.RegisterCallback<ClickEvent>(DisplaySaveConfirmation);
        _SaveSlot5.RegisterCallback<ClickEvent>(DisplaySaveConfirmation);
        
        // ...When pressing yes/no on the confirmation
        _ButtonNoSaveConfirmation.RegisterCallback<ClickEvent>(CloseSaveConfirmation);
        _ButtonYesSaveConfirmation.RegisterCallback<ClickEvent>(PressedYesOnSaveConfirmation);
    }

    private void DisplaySaveConfirmation(ClickEvent evt)
    {
        _SaveConfirmation.style.display = DisplayStyle.Flex;
    }

    private void CloseSaveConfirmation(ClickEvent evt)
    {
        _SaveConfirmation.style.display = DisplayStyle.None;
    }

    private void PressedYesOnSaveConfirmation(ClickEvent evt)
    {
        // TODO: Save game
        
        // Close SaveConfirmation window
        _SaveConfirmation.style.display = DisplayStyle.None;
    }
}

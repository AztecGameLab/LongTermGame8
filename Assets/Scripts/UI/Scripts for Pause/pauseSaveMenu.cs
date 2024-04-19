using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class pauseSaveMenu : MonoBehaviour
    {
        private const int NumSaveSlots = 6;

        // References
        private Button[] _saveSlots;
    
        private VisualElement _saveConfirmation;

        private Button _buttonYesSaveConfirmation;
        private Button _buttonNoSaveConfirmation;

        private Label _confirmationText;
        private int _selectedSlot;
    
        // Start is called before the first frame update
        private void Start()
        {
            // Getting root to reach the other elements of UI document
            var root = GetComponent<UIDocument>().rootVisualElement;
        
            // Getting references for...
        
            // Save Slots...
            _saveSlots = new Button[NumSaveSlots];
            for (var i = 0; i < NumSaveSlots; i++)
            {
                var saveSlot = root.Q<Button>("SaveSlot" + i);
                _saveSlots[i] = saveSlot;
                var slot = i;
                saveSlot.clicked += () => DisplaySaveConfirmation(slot);
            }
        
            // Save confirmation window
            _saveConfirmation = root.Q<VisualElement>("SaveConfirmation");

            _confirmationText = root.Q<Label>("ConfirmationText");
        
            // Yes and no buttons for save confirmation...
            _buttonYesSaveConfirmation = root.Q<Button>("ButtonYes");
            _buttonNoSaveConfirmation = root.Q<Button>("ButtonNo");
        
            // ...When pressing yes/no on the confirmation
            _buttonNoSaveConfirmation.clicked += CloseSaveConfirmation;
            _buttonYesSaveConfirmation.clicked += PressedYesOnSaveConfirmation;
        }

        private void DisplaySaveConfirmation(int saveSlot)
        {
            _selectedSlot = saveSlot;
            _confirmationText.text = $"Are you sure you want to overwrite save slot #{saveSlot}?";
            _saveConfirmation.style.display = DisplayStyle.Flex;
        }

        private void CloseSaveConfirmation()
        {
            _saveConfirmation.style.display = DisplayStyle.None;
        }

        private void PressedYesOnSaveConfirmation()
        {
            // TODO: Save game
        
            // Close SaveConfirmation window
            _saveConfirmation.style.display = DisplayStyle.None;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class PauseLoadMenu : MonoBehaviour
    {
        private const int NumSaveSlots = 6;

        private DiskSaveSerializer _saveSerializer;
        
        // References
        private Button[] _saveSlots;
    
        private VisualElement _loadConfirmation;

        private Button _buttonYesSaveConfirmation;
        private Button _buttonNoSaveConfirmation;

        private Label _loadConfirmationText;
        private int _selectedSlot;
    
        // Start is called before the first frame update
        private void Start()
        {
            _saveSerializer = FindAnyObjectByType<DiskSaveSerializer>();
            
            // Getting root to reach the other elements of UI document
            var root = GetComponent<UIDocument>().rootVisualElement;
        
            // Getting references for...
        
            // Save Slots...
            _saveSlots = new Button[NumSaveSlots];
            for (var i = 0; i < NumSaveSlots; i++)
            {
                var saveSlot = root.Q<Button>("LoadSlot" + i);
                _saveSlots[i] = saveSlot;
                var slot = i;
                saveSlot.clicked += () => DisplayLoadConfirmation(slot);
            }
        
            // Save confirmation window
            _loadConfirmation = root.Q<VisualElement>("LoadConfirmation");

            _loadConfirmationText = root.Q<Label>("LoadConfirmationText");
        
            // Yes and no buttons for save confirmation...
            _buttonYesSaveConfirmation = root.Q<Button>("LoadYes");
            _buttonNoSaveConfirmation = root.Q<Button>("LoadNo");
        
            // ...When pressing yes/no on the confirmation
            _buttonNoSaveConfirmation.clicked += CloseLoadConfirmation;
            _buttonYesSaveConfirmation.clicked += PressedYesOnLoadConfirmation;
        }

        private void DisplayLoadConfirmation(int loadSlot)
        {
            _selectedSlot = loadSlot;
            _loadConfirmationText.text = $"Are you sure you want to load slot #{loadSlot}?\nThis will overwrite any unsaved progress.";
            _loadConfirmation.style.display = DisplayStyle.Flex;
        }

        private void CloseLoadConfirmation()
        {
            _loadConfirmation.style.display = DisplayStyle.None;
        }

        private async void PressedYesOnLoadConfirmation()
        {
            // TODO: Load game
            await _saveSerializer.ReadFromDisk(_selectedSlot.ToString());
        
            // Close LoadConfirmation window
            CloseLoadConfirmation();
        }
    }
}
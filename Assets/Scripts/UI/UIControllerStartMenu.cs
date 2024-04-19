using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ltg8
{
    public class UIControllerStartMenu : MonoBehaviour
    {
        // References
        [SerializeField] private VisualElement _SideMenu;
        [SerializeField] private VisualElement _ButtonContainer;
        [SerializeField] private Button _ButtonStart;
        [SerializeField] private VisualElement _ScreenSpace;

        // For anyone wanting to do stuff with the other stuff, do the same as was done for these first three 

        void Start()
        {
            // Getting the root of the hirearchy to search for needed vars
            var root = GetComponent<UIDocument>().rootVisualElement;

            // Getting references
            _SideMenu = root.Q<VisualElement>("SideMenu");
            _ButtonContainer = root.Q<VisualElement>("ButtonContainer");
            _ScreenSpace = root.Q<VisualElement>("ScreenSpace");
            _ButtonStart = root.Q<Button>("ButtonStart");


            _ButtonStart.RegisterCallback<ClickEvent>(WhenStartIsPressed);
        }

        private void WhenStartIsPressed(ClickEvent evt)
        {
            // Slide SideMenu to the side (lol). translat should be >= 650
            _SideMenu.AddToClassList("SideMenuSlidingOut");

            // Fade screen black
            _ScreenSpace.AddToClassList("ScreenSpaceGoesDark");
            
            // switch scenes (This line could be better...). The line
            // bottom to these comments calls the method after 4 seconds
            Invoke(nameof(SwitchScenesAfterStartIsPressed), 4f);
        }

        private async void SwitchScenesAfterStartIsPressed()
        {
            // LOAD THE FIRST SCENE HERE!!!!!
            Debug.Log("Switch to desired scene");
            await Ltg8.GameState.TransitionTo(new OverworldGameState(Ltg8.Save.PlayerSceneName));
        }
    }
}
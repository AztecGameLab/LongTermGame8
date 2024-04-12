using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Ltg8.Player
{
    public class PauseView : MonoBehaviour
    {

        [SerializeField] private GameObject pauseMenu;
        
        public UnityEvent onClose;

        public void Open()
        {
            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }

        public void Close()
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                
                onClose.Invoke();
            }
        }

        public void QuitToMenu()
        {
            string path = Ltg8.Settings.mainMenuScenePath;
            SceneManager.LoadScene(path);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
        }
        
    }
    
    
}

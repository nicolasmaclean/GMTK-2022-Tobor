using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.Events;

namespace Game.UI
{
    public class GameMenuController : MonoBehaviour
    {
        public static bool Paused = false;
        
        [Header("Controls")]
        KeyCode _pauseKey = KeyCode.Escape;
        
        [Header("Events")]
        public UnityEvent OnPause;
        public UnityEvent OnResume;
        
        public static Action S_OnPause;
        public static Action S_OnResume;

        [Header("Menus")]
        [SerializeField]
        GameObject _pauseMenu;

        void Awake()
        {
            Time.timeScale = 1;
            _pauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(_pauseKey))
            {
                if (Paused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        
        public void Resume()
        {
            Paused = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _pauseMenu.SetActive(false);
            
            Time.timeScale = 1;
            OnResume?.Invoke();
            S_OnResume?.Invoke();
        }

        public void Pause()
        {
            Paused = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            _pauseMenu.SetActive(true);
            
            Time.timeScale = 0;
            OnPause?.Invoke();
            S_OnPause?.Invoke();
        }

        public void GoToMainMenu()
        {
            SceneController.LoadScene(GameScene.Start);
            Paused = false;
        }

        public void Quit()
        {
            SceneController.Quit();
        }
    }
}

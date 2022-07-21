using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.UI.Hud;
using UnityEngine.Events;

namespace Game.UI
{
    public class GameMenuController : MonoBehaviour
    {
        public static bool Paused = false;
        public static GameMenuController Instance;
        
        [Header("Controls")]
        KeyCode _pauseKey = KeyCode.Escape;
        
        [Header("Events")]
        public UnityEvent OnPause;
        public UnityEvent OnResume;
        public UnityEvent OnLose;
        
        [Header("Menus")]
        [SerializeField]
        GameObject _pauseMenu;
        
        [SerializeField]
        GameObject _loseMenu;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Time.timeScale = 1;
            
            _pauseMenu.SetActive(false);
            _loseMenu.SetActive(false);
        }

        void OnDestroy()
        {
            if (Instance != null) return;
            
            Instance = null;
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
            Crosshair.Show();
            
            _pauseMenu.SetActive(false);
            
            Time.timeScale = 1;
            OnResume?.Invoke();
        }

        public void Pause()
        {
            Paused = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Crosshair.Hide();
            
            _pauseMenu.SetActive(true);
            
            Time.timeScale = 0;
            OnPause?.Invoke();
        }

        public static void Lose()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Crosshair.Hide();
            
            Instance._loseMenu.SetActive(true);
            
            Time.timeScale = 0;
            Instance.OnLose?.Invoke();
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1;
            Paused = false;
            SceneController.LoadScene(GameScene.Start);
        }

        public void Quit()
        {
            SceneController.Quit();
        }
    }
}

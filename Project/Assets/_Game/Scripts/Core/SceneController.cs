using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// Lazy-loaded singleton
        /// </summary>
        public static SceneController Instance
        {
            get
            {
                if (instance == null)
                {
                    Create();
                }

                return instance;
            }
        }

        static SceneController instance = null;

        static void Create()
        {
            GameObject go = new GameObject("S_SceneController");
            instance = go.AddComponent<SceneController>();
        }

        Scene currentScene;
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            currentScene = SceneManager.GetActiveScene();
        }

        public void LoadNextScene() => LoadScene(currentScene.buildIndex + 1);

        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
            currentScene = SceneManager.GetActiveScene();
        }

        public void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }
}

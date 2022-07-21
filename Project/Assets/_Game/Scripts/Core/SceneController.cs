using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public static class SceneController
    {
        static Scene currentScene
        {
            get
            {
                return SceneManager.GetActiveScene();
            }
        }

        public static void LoadNextScene() => LoadScene(currentScene.buildIndex + 1);
        public static void LoadLastScene() => LoadScene(currentScene.buildIndex - 1);

        static void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public static void LoadScene(GameScene scene)
        {
            LoadScene((int) scene);
        }

        public static void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }

    public enum GameScene
    {
        Start = 0, Character = 1, Game = 2
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.UI.Utility
{
    public class StartMenuController : MonoBehaviour
    {
        public void Play()
        {
            SceneController.LoadNextScene();
        }

        public void Credits()
        {
            SceneController.LoadScene(GameScene.Credits);
        }

        public void Quit()
        {
            SceneController.Quit();
        }
    }
}

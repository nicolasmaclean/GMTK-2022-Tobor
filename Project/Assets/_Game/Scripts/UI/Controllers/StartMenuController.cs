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
            SceneController.Instance.LoadNextScene();
        }

        public void Quit()
        {
            SceneController.Instance.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.SceneManagement;

namespace Game.UI.Utility
{
    public class PauseMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Resume()
        {
            Time.timeScale = 1;
        }

        public void GoBackToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}

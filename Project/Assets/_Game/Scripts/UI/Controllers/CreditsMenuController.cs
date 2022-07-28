using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.UI.Utility
{
    public class CreditsMenuController : MonoBehaviour
    {
        public void Exit()
        {
            SceneController.LoadScene(GameScene.Start);
        }
    }
}

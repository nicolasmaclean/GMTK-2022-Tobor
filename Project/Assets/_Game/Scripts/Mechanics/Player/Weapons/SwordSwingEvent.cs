using UnityEngine;

namespace Game.Mechanics.Player
{
    public class SwordSwingEvent : MonoBehaviour
    {
        public void Swing() => PlayerController.Instance.SwordSwing();
    }
}
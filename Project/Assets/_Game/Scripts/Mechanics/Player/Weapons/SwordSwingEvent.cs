using System;
using UnityEngine;

namespace Game.Mechanics.Player
{
    public class SwordSwingEvent : MonoBehaviour
    {
        Collider _collider;

        void Awake()
        {
            _collider = GetComponentInChildren<Collider>();
            _collider.gameObject.SetActive(false);
        }

        public void Swing() => PlayerController.Instance.SwordSwing();

        public void EnableCollider() => _collider.gameObject.SetActive(true);
        public void DisableCollider() => _collider.gameObject.SetActive(false);
    }
}
using System;
using Game.Utility;
using UnityEngine;

namespace Game.Mechanics.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStats _stats;

        FPSController _playerController;

        void Awake()
        {
            _playerController = GetComponent<FPSController>();
        }
        
        void Start()
        {
            _playerController.UpdateSpeed(_stats.Agility);
        }
    }
}
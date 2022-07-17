using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Mechanics.Player;
using UnityEditor;
using UnityEngine;

namespace Game.Mechanics.Level
{
    public class LevelController : MonoBehaviour
    {
        public static RoomBase CurrentRoom { get; private set; }
        
        [SerializeField]
        RoomBase[] _rooms;
        
        static int _currentRoomIndex;

        void Awake()
        {
            CurrentRoom = _rooms[0];
            _currentRoomIndex = 0;

            DisableAllButStart();
        }

        void DisableAllButStart()
        {
            for (int i = 2; i < _rooms.Length; i++)
            {
                _rooms[i].Hide();
            }
            
            _rooms[0].Load();
        }

        public void MoveToNext()
        {
            if (_currentRoomIndex == _rooms.Length - 1)
            {
                PlayerController.Instance.WinGame();
            }
            
            // hide rooms
            if (_currentRoomIndex - 1 >= 0)
            {
                _rooms[_currentRoomIndex - 1].Hide();
            }
            
            // close the last door
            CurrentRoom.Close();
            
            // update current room pointer
            _currentRoomIndex++;
            if (_currentRoomIndex >= _rooms.Length) return;
            CurrentRoom = _rooms[_currentRoomIndex];

            // load next room
            if (_currentRoomIndex + 1 < _rooms.Length)
            {
                _rooms[_currentRoomIndex + 1].Load();
            }
        }
    }
}
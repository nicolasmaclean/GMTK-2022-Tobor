using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace Game.Mechanics.Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        RoomBase[] _rooms;
        
        [SerializeField]
        [Utility.ReadOnly]
        RoomBase _currentRoom;
        int _currentRoomIndex;

        void Awake()
        {
            _currentRoom = _rooms[0];
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
            Debug.Log("Next");
            // hide rooms
            if (_currentRoomIndex - 1 >= 0)
            {
                _rooms[_currentRoomIndex - 1].Hide();
            }
            
            // close the last door
            _currentRoom.Close();
            
            // update current room pointer
            _currentRoomIndex++;
            if (_currentRoomIndex >= _rooms.Length) return;
            _currentRoom = _rooms[_currentRoomIndex];

            // load next room
            if (_currentRoomIndex + 1 < _rooms.Length)
            {
                _rooms[_currentRoomIndex + 1].Load();
            }
        }
    }
}
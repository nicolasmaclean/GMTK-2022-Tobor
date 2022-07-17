using System;
using UnityEngine;

namespace Game.Mechanics.Level
{
    public class Buffer : RoomBase
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                Done();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

namespace Game.Mechanics.Level
{
    public class DiceTowerTrigger : MonoBehaviour
    {
        public bool PlayerInside { get; set; }

        void OnTriggerEnter(Collider other)
        {
            if (!PlayerInside && IsPlayer(other))
            {
                PlayerInside = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (PlayerInside && IsPlayer(other))
            {
                PlayerInside = false;
            }
        }

        bool IsPlayer(Collider other)
        {
            Transform parent = other.transform.parent;
            if (!parent) return false;

            return parent.GetComponent<PlayerController>();
        }
    }
}
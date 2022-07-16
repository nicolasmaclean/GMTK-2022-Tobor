using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    public class DamageCollision : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"The player is attacked");
        }
    }
}

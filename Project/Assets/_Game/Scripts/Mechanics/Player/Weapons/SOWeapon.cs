using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

namespace Game.Mechanics.Player
{
    [CreateAssetMenu(menuName = "Data/Weapon")]
    public class SOWeapon : ScriptableObject
    {
        public WeaponType WeaponType;
        
        [Tooltip("Cooldown includes the duration of the attack.")]
        public float Cooldown = .6f;
        public float Damage = 1f;
        public float Speed = 1f;
    }
    
    public enum WeaponType
    {
        Sword = 0, Bow = 1,
    }
}

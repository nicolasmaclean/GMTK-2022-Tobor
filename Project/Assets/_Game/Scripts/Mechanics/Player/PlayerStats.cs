using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Mechanics.Player
{
    [System.Serializable]
    public class PlayerStats
    {
        public String PlayerName = "Player";
        
        [Range(1, 20)]
        public int Constitution = 1;

        [Range(1, 20)]
        public int Strength = 1;

        [Range(1, 20)]
        public int Agility = 1;

        [Range(1, 20)]
        public int Luck = 1;

        [Range(1, 20)]
        public int Perception = 1;

        public WeaponType Weapon = WeaponType.Sword;
        
        public static PlayerStats CreateRandom()
        {
            PlayerStats stats = new PlayerStats();
            
            stats.Constitution = Random.Range(1, 21);
            stats.Strength = Random.Range(1, 21);
            stats.Agility = Random.Range(1, 21);
            stats.Luck = Random.Range(1, 21);
            stats.Perception = Random.Range(1, 21);

            return stats;
        }

        public static String[] GetFieldNames()
        {
            return new []
            {
                nameof(Strength),
                nameof(Agility),
                nameof(Constitution),
                nameof(Perception),
                nameof(Luck)
            };
        }
    }

    public enum WeaponType
    {
        Sword = 0, Bow = 1, Magic = 2
    }
}
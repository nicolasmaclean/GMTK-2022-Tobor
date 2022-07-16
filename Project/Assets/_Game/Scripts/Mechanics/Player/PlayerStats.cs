using UnityEngine;

namespace Game.Mechanics.Player
{
    [System.Serializable]
    public class PlayerStats
    {
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
    }

    public enum WeaponType
    {
        Sword = 0, Bow = 1, Magic = 2
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Mechanics.Player
{
    [Serializable]
    public class PlayerStats
    {
        public static PlayerStats Instance
        {
            get
            {
                if (instance == null) instance = CreateRandom();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        static PlayerStats instance = null;

        public String PlayerName = "Player";

        #region Data
        [Range(1, 20)]
        public int Constitution = 1;
        public Vector2 ConstitutionRange = new Vector2(1, 10);

        [Range(1, 20)]
        public int Strength = 1;
        public Vector2 StrengthRange = new Vector2(1, 5);

        [Range(1, 20)]
        public int Agility = 1;
        public Vector2 AgilityRange = new Vector2(8, 24);

        [Range(1, 20)]
        public int Luck = 1;
        public Vector2 LuckRange = new Vector2(1, 10);

        [Range(1, 20)]
        public int Perception = 1;
        public Vector2 PerceptionRange = new Vector2(.4f, .1f);
        #endregion
        
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
                // nameof(Luck)
            };
        }

        public static float GetInRange(int val, Vector2 range)
        {
            return Mathf.LerpUnclamped(range.x, range.y, val / 20f);
        }
    }
}
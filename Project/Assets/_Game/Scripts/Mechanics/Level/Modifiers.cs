using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Level
{
    public static class Modifiers
    {
        const int RollsToMax = 5;
        public static int EnemyMultiplier { get; private set; } = 1;
        
        const float MaxDamageMultiplier = 4;
        public static float DamageMultiplier { get; private set; } = 1;
        
        const float MaxSpeedMultiplier = 2;
        public static float SpeedMultiplier { get; private set; } = 1;
        
        const float MaxAttackSpeedMultiplier = 4;
        public static float AttackSpeedMultiplier { get; private set; } = 1;
        
        const float MaxJumpMultiplier = 4;
        public static float JumpMultiplier { get; private set; } = 1;
        public static int Heal { get; private set; } = 0;

        public static Action OnChange;

        public static void SetMultipliers(int enemy, int damage, int speed, int attack, int jump, int heal)
        {
            EnemyMultiplier         += 2 * enemy - EnemyMultiplier == 1 ? 1 : 0;
            DamageMultiplier        = Mathf.Clamp(DamageMultiplier + damage * ((MaxDamageMultiplier - 1) / RollsToMax), 1, MaxDamageMultiplier);
            SpeedMultiplier         = Mathf.Clamp(SpeedMultiplier + speed * ((MaxSpeedMultiplier - 1) / RollsToMax), 1, MaxSpeedMultiplier);
            AttackSpeedMultiplier   = Mathf.Clamp(AttackSpeedMultiplier + attack * ((MaxAttackSpeedMultiplier - 1) / RollsToMax), 1, MaxAttackSpeedMultiplier);
            JumpMultiplier          = Mathf.Clamp(JumpMultiplier + jump * ((MaxJumpMultiplier - 1)/ RollsToMax), 1, MaxJumpMultiplier);
            Heal                    = heal;
            
            OnChange?.Invoke();
        }
    }
}
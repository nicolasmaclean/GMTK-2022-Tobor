using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Modifiers
{
    public static int EnemyMultiplier { get; private set; } = 1;
    public static int DamageMultiplier { get; private set; } = 1;
    public static int SpeedMultiplier { get; private set; } = 1;
    public static int AttackSpeedMultiplier { get; private set; } = 1;
    public static int JumpMultiplier { get; private set; } = 1;
    public static int BaseStatsMultiplier { get; private set; } = 1;

    public static Action OnChange;

    public static void SetMultipliers(int enemy, int damage, int speed, int attack, int jump, int stats)
    {
        EnemyMultiplier =       (int) Mathf.Pow(2 , enemy);
        DamageMultiplier =      (int) Mathf.Pow(2 , damage);
        SpeedMultiplier =       (int) Mathf.Pow(2 , speed);
        AttackSpeedMultiplier = (int) Mathf.Pow(2 , attack);
        JumpMultiplier =        (int) Mathf.Pow(2 , jump);
        BaseStatsMultiplier =   (int) Mathf.Pow(2 , stats);
        
        OnChange?.Invoke();
    }
}

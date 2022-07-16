using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //stats
    [SerializeField]
    protected int _health;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected float _damageRate;
    [SerializeField]
    protected float _rangeOfAttack;
    [SerializeField]
    protected EnemyType _enemyType;

    protected enum EnemyType
    {
        MeleeAttacker, RangedAttacker, FastChargingAttacker, PassiveAttacker, FlyingAttacker
    }

    //attack
    public virtual void EnemyDamaged(int damage)
    {
        _health -= damage;
    }

    public virtual void EnemyDied()
    {
        Debug.Log("Enemy is dead");
    }

}

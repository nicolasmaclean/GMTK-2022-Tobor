using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //stats
    protected int _health;
    protected int _attack;
    protected float _damageRate;
    protected float _rangeOfAttack;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

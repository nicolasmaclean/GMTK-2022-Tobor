using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Enemy;
using Game.Mechanics.Player;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    PlayerController _player;

    void Awake()
    {
        _player = PlayerController.Instance;        
    }
    
    void OnTriggerEnter(Collider other)
    {
        var parent = other.transform.parent;
        if (!parent) return;
        
        EnemyBase em = parent.GetComponent<EnemyBase>();
        if (!em) return;
    
        em.Harm((int) _player.Weapon.Damage);
    }
}
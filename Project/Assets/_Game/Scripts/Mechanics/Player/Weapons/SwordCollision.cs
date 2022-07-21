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
        EnemyBase em = other.GetComponentInParent<EnemyBase>();
        if (!em) return;
    
        em.Harm((int) _player.Sword.Damage);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Enemy;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Events;

public class SwordCollision : MonoExtended
{
    public UnityEvent OnHit;
    PlayerController _player;

    void Start()
    {
        _player = PlayerController.Instance;        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.gameObject.layer == LayerMask.GetMask("Player")) return;
        
        OnHit?.Invoke();
        
        EnemyBase em = other.GetComponentInParent<EnemyBase>();
        if (!em) return;

        Vector3 hitPosition = other.ClosestPoint(transform.position);
        Vector3 hitNormal = (_player.transform.position - em.transform.position).normalized;
        em.Harm((int) _player.Sword.Damage, hitPosition, hitNormal);
    }
}
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
        gameObject.SetActive(false);
    }

    void Start()
    {
        _player = PlayerController.Instance;        
    }
    
    void OnTriggerEnter(Collider other)
    {
        EnemyBase em = other.GetComponentInParent<EnemyBase>();
        if (!em) return;

        Vector3 hitPosition = other.ClosestPoint(transform.position);
        Vector3 hitNormal = (_player.transform.position - em.transform.position).normalized;
        em.Harm((int) _player.Sword.Damage, hitPosition, hitNormal);
    }
}
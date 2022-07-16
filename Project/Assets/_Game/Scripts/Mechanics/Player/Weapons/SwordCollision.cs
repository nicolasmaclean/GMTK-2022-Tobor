using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [SerializeField]
    PlayerController _player;
    
    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent) return;
        
        Enemy em = other.transform.parent.GetComponent<Enemy>();
        if (!em) return;
    
        em.EnemyDamaged((int) _player.Weapon.Damage);
        Debug.Log("Hit Enemy");
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    [SelectionBase]
    public abstract class Enemy : MonoBehaviour
    {
        public float Health { get; protected set; }
        
        [Header("Stats")]
        [SerializeField]
        protected float _baseHealth = 10f;
        
        [SerializeField]
        protected float _attack = 1f;
        
        [SerializeField]
        protected float _damageRate = 2f;
        
        [SerializeField]
        protected float _rangeOfAttack = 3f;

        protected PlayerController _player;

        void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            Health = _baseHealth;
            _player = PlayerController.Instance;
        }

        public virtual void Harm(int damage)
        {
            Health -= damage;
            
            if (Health <= 0)
            {
                Kill();
            }
        }

        protected virtual void Kill()
        {
            Debug.Log("Enemy is dead");
            gameObject.SetActive(false);
        }
    }
}

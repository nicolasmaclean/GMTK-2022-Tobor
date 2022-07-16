using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    [SelectionBase]
    public abstract class EnemyBase : MonoBehaviour
    {
        public float Health { get; protected set; }
        public Action<EnemyBase> OnKilled;

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

        void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake()
        {
            Health = _baseHealth;
        }

        protected virtual void OnStart()
        {
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
            OnKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}

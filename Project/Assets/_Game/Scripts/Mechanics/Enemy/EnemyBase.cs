using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

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

        [Header("Events")]
        [SerializeField]
        public UnityEvent OnHurt;
        public UnityEvent OnDead;

        protected PlayerController _player;
        protected AnimatedSprite _anim;
        protected bool isHarmed;


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
            isHarmed = false;
            _anim = transform.GetComponentInChildren<AnimatedSprite>();
        }

        protected virtual void OnStart()
        {
            _player = PlayerController.Instance;
        }

        public virtual void Harm(float damage)
        {
            Health -= damage * Modifiers.DamageMultiplier;
            isHarmed = true;
            OnHurt?.Invoke();
            
            if (Health <= 0)
            {
                Kill();
            }
        }

        protected virtual void Kill()
        {
            OnDead?.Invoke();
            Debug.Log("Enemy is dead");
            OnKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
        
        protected static IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }

        public void PlaySFX(SOAudioClip clip)
        {
            SFXManager.PlaySFX(clip);
        }
    }
}

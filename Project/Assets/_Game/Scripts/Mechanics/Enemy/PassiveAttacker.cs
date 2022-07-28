using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Mechanics.Enemy
{
    public class PassiveAttacker : EnemyBase
    {
        [SerializeField]
        SOSpriteAnimation _idleAnimation;
        
        [SerializeField]
        SOSpriteAnimation _attackAnimation;
        
        [SerializeField]
        SOSpriteAnimation _transformAnimation;
        
        [Header("More Events!")]
        [SerializeField]
        public UnityEvent OnAttack;

        [Header("Colliders")]
        [SerializeField]
        Collider _deafultCollider;
        
        [SerializeField]
        Collider _aggressiveCollider;

        PlayerTrigger _playerTrigger;
        bool _passive = true;

        protected override void OnAwake()
        {
            base.OnAwake();
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
        }

        protected override void OnStart()
        {
            _anim.LoadAnimationRandom(_idleAnimation);
            _aggressiveCollider.gameObject.SetActive(false);
        }

        public override void Harm(float damage, Vector3 hitPosition, Vector3 hitNormal)
        {
            base.Harm(damage, hitPosition, hitNormal);
            if (_passive)
            {
                _passive = false;
                
                _deafultCollider.gameObject.SetActive(false);
                _aggressiveCollider.gameObject.SetActive(true);
                
                _anim.PlayOneShot(_transformAnimation, () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                    StartCoroutine(SeekLoop());
                });
            }
        }

        protected override void EnemyAttack()
        {
            _agent.isStopped = true;
            if (_playerTrigger.PlayerIsIn)
            {
                PlayerController.Instance.Hurt(_attack);
            }

            OnAttack?.Invoke();
            _anim.PlayOneShot(_attackAnimation, speedMultiplier: Modifiers.AttackSpeedMultiplier, callback: () =>
            {
                _anim.LoadAnimation(_walkAnimation);
                _agent.isStopped = false;
            });
        }
    }
}

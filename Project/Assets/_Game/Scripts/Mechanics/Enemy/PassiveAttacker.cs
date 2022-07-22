using System;
using System.Collections;
using System.Collections.Generic;
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
        }

        public override void Harm(float damage)
        {
            base.Harm(damage);
            if (_passive)
            {
                _passive = false;
                _anim.LoadAnimation(_transformAnimation);
                StartCoroutine(WaitThen(_anim.Length, () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                    StartCoroutine(SeekLoop());
                }));
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

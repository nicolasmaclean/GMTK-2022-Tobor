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
        float _stampForNextAttack;

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

        protected override void DetectPlayer()
        {
            float currentTargetDistance = Vector3.Distance(transform.position, _player.transform.position);
            if (currentTargetDistance <= _rangeOfAttack)
            {
                _agent.isStopped = true;
                if (Time.time > _stampForNextAttack)
                {
                    _stampForNextAttack = Time.time + _damageRate;
                    EnemyAttack();
                }
            }
            else
            {
                _agent.isStopped = false;
            }
        }

        void EnemyAttack()
        {
            if (_playerTrigger.PlayerIsIn)
            {
                PlayerController.Instance.Hurt(_attack);
            }

            OnAttack?.Invoke();
            _anim.PlayOneShot(_attackAnimation, () =>
            {
                _anim.LoadAnimation(_walkAnimation);
            });
        }
    }
}

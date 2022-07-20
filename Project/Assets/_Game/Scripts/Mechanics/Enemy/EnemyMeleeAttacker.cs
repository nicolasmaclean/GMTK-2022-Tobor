using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMeleeAttacker : EnemyBase
    {
        [SerializeField]
        SOSpriteAnimation _attackAnimation;
        
        [Header("More Events!")]
        [SerializeField]
        public UnityEvent OnAttack;

        PlayerTrigger _playerTrigger;
        float _stampForNextAttack;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
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
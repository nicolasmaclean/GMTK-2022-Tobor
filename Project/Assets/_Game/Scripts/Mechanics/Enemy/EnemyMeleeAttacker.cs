using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
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
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
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
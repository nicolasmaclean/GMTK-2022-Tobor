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
    public class EnemyBoss : EnemyBase
    {
        [SerializeField]
        SOSpriteAnimation _attackAnimation;

        [SerializeField]
        SOSpriteAnimation _shootAnimation;

        [SerializeField]
        [Tooltip("The number of frames into the shoot animation to wait before spawning a bullet.")]
        float _shootDelay = 3;

        [Header("More Stats!")]
        [SerializeField]
        float _meleeRange = 10f;
        
        [SerializeField]
        Transform _bulletSpawnPoint;
        
        [SerializeField]
        GameObject _bullet;
        
        
        [Header("More Events")]
        public UnityEvent OnAttack;
        public UnityEvent OnShoot;

        PlayerTrigger _playerTrigger;

        protected override void OnAwake()
        {
            base.OnAwake();
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
        }
        
        protected override void EnemyAttack()
        {
            _agent.isStopped = true;
            float currentTargetDistance = Vector3.Distance(transform.position, _player.transform.position);
            if (currentTargetDistance < _meleeRange)
            {
                EnemyMelee();
            }
            else
            {
                EnemyRanged();
            }
        }

        void EnemyMelee()
        {
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

        void EnemyRanged()
        {
            _anim.PlayOneShot(_shootAnimation, speedMultiplier: Modifiers.AttackSpeedMultiplier, callback:() =>
            {
                _anim.LoadAnimation(_walkAnimation);
                _agent.isStopped = false;
            });

            StartCoroutine(WaitThen((_anim.Spf / Modifiers.AttackSpeedMultiplier) * _shootDelay, () =>
            {
                OnShoot?.Invoke();
                var bulletSpawn = _bulletSpawnPoint.transform;
                Instantiate(_bullet, bulletSpawn.position, bulletSpawn.rotation);
            }));
        }
    }
}

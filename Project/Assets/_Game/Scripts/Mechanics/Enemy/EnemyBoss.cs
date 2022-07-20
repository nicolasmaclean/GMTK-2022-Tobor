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
        float _rangeOfShot = 6f;
        
        [SerializeField]
        Transform _bulletSpawnPoint;
        
        [SerializeField]
        GameObject _bullet;
        
        [SerializeField]
        float _fireRate = 3f;
        
        [Header("More Events")]
        public UnityEvent OnAttack;
        public UnityEvent OnShoot;

        PlayerTrigger _playerTrigger;
        float _stampForNextAttack;
        float _stampForNextShot;
        bool canShoot = true;

        protected override void OnAwake()
        {
            base.OnAwake();
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
        }
        
        protected override void DetectPlayer()
        {
            float currentTargetDistance = Vector3.Distance(transform.position, _player.transform.position);
            if (currentTargetDistance <= _rangeOfShot)
            {
                if (Time.time > _stampForNextShot)
                {
                    _stampForNextShot = Time.time + _fireRate;
                    EnemyShoot(canShoot);
                }
                if (currentTargetDistance <= _rangeOfAttack)
                {
                    _agent.isStopped = true;
                    canShoot = false;
                    if (Time.time > _stampForNextAttack)
                    {
                        _stampForNextAttack = Time.time + _damageRate;
                        EnemyAttack();
                    }
                }
                else
                {
                    canShoot = true;
                    _agent.isStopped = false;
                }
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

        void EnemyShoot(bool rangeCheck)
        {
            if (rangeCheck)
            {
                _anim.PlayOneShot(_shootAnimation, () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                });

                StartCoroutine(WaitThen(_anim.Spf * _shootDelay, () =>
                {
                    OnShoot?.Invoke();
                    Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
                }));
            }
        }
    }
}

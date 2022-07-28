using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using UnityEngine;
using UnityEngine.AI;
using Game.Mechanics.Player;
using Game.Utility;
using UnityEngine.Events;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyRangedAttacker : EnemyBase
    {
        [SerializeField]
        SOSpriteAnimation _attackAnimation;
        
        [Header("Ranged")]
        [SerializeField]
        Transform _bulletSpawnPoint;
        public UnityEvent OnShoot;

        [SerializeField]
        GameObject _bullet;

        [SerializeField]
        int _shootDelay = 3;

        bool _waiting = false;
        protected override void DetectPlayer()
        {
            Vector3 dir = transform.position - _player.transform.position;
            float distSqr = dir.sqrMagnitude;
            if (distSqr <= _rangeSqr)
            {
                float cooldown = _attackSpeed / Modifiers.AttackSpeedMultiplier;
                if (_lastAttack > cooldown)
                {
                    _lastAttack = 0;
                    EnemyAttack();
                }
                else if (!_waiting)
                {
                    _waiting = true;
                    _agent.isStopped = true;
                    StartCoroutine(Coroutines.WaitThen(cooldown - _lastAttack, () =>
                    {
                        _waiting = false;
                        _agent.isStopped = false;
                    }));
                }
            }
        }

        protected override void EnemyAttack()
        {
            _agent.isStopped = true;
            _anim.PlayOneShot(_attackAnimation, speedMultiplier: Modifiers.AttackSpeedMultiplier, callback: () =>
            {
                _anim.LoadAnimation(_walkAnimation);
                _agent.isStopped = false;
            });

            StartCoroutine(WaitThen((_anim.Spf / Modifiers.AttackSpeedMultiplier) * _shootDelay, () =>
            {
                OnShoot?.Invoke();
                EnemyBullet bullet = Instantiate(_bullet, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation).GetComponent<EnemyBullet>();
                bullet._damage = _attack;
            }));
        }
    }
}
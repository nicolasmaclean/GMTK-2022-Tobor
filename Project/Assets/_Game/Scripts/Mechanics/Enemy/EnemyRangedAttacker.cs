using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

        float _stampForNextAttack;

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
            _anim.PlayOneShot(_attackAnimation, () =>
            {
                _anim.LoadAnimation(_walkAnimation);
            });

            StartCoroutine(WaitThen(_anim.Spf * 3, () =>
            {
                OnShoot?.Invoke();
                EnemyBullet bullet = Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation).GetComponent<EnemyBullet>();
                bullet._damage = _attack;
            }));
        }
    }
}
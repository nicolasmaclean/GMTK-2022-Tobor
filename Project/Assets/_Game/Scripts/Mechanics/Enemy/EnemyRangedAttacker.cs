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
        [Header("Ranged")]
        [SerializeField]
        Transform _bulletSpawnPoint;
        public UnityEvent OnShoot;

        [SerializeField]
        GameObject _bullet;

        [Header("Animation")]
        [SerializeField]
        SOSpriteAnimation _walkAnimation;
        
        [SerializeField]
        SOSpriteAnimation _attackAnimation;

        readonly float SEARCH_INTERVAL = 0.2f;

        NavMeshAgent _agent;
        
        float _stampForNextAttack;

        protected override void OnAwake()
        {
            base.OnAwake();
            _agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartCoroutine(SeekLoop());
        }

        IEnumerator SeekLoop()
        {
            while (true)
            {
                _agent.SetDestination(_player.transform.position);
                DetectPlayer();
                
                yield return new WaitForSeconds(SEARCH_INTERVAL);
            }
        }

        void DetectPlayer()
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
            _anim.LoadAnimation(_attackAnimation);
            _anim.PlayOneShot(_attackAnimation, () =>
            {
                _anim.LoadAnimation(_walkAnimation);
            });

            StartCoroutine(WaitThen(_anim.Spf * 3, () =>
                {
                    OnShoot?.Invoke();
                    Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
                }
            ));
        }
    }
}
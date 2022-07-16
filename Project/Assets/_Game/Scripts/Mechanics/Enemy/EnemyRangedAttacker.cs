using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyRangedAttacker : EnemyBase
    {
        [Header("Ranged")]
        [SerializeField]
        Transform _bulletSpawnPoint;
        [SerializeField]
        Transform _bulletSpawnPointAnchor;

        [SerializeField]
        GameObject _bullet;

        readonly float SEARCH_INTERVAL = 0.2f;

        NavMeshAgent _agent;
        
        float _timeStamp = 0f;
        float _timeDelay = 0.2f;

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

        void Update()
        {
            _bulletSpawnPointAnchor.LookAt(_player.transform.position);
            //chase player
            if (Time.time >= _timeStamp + _timeDelay)
            {
                _agent.SetDestination(_player.transform.position);
                _timeStamp = Time.time;
                DetectPlayer();
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
            Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
        }
    }
}
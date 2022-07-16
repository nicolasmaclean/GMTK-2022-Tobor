using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMeleeAttacker : Enemy
    {
        [Header("Melee")]
        [SerializeField] GameObject _attackCollider;

        readonly float SEARCH_INTERVAL = 0.2f;
        
        NavMeshAgent _agent;
        float _stampForNextAttack;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _agent = GetComponent<NavMeshAgent>();
        }
        
        void Start()
        {
            _attackCollider.SetActive(false);
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
            Collider[] hitPlayers = Physics.OverlapBox(_attackCollider.transform.position, _attackCollider.transform.position);
            foreach (Collider player in hitPlayers)
            {
                Debug.Log("Attacked Player");
            }
        }
    }
}
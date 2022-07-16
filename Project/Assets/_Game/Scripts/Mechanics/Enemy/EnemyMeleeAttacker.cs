using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMeleeAttacker : Enemy
    {
        [Header("Melee")]
        [SerializeField] GameObject _attackCollider;

        NavMeshAgent _agent;
        
        float _timeStamp = 0f;
        float _timeDelay = 0.2f;

        float _nextAttack = 0f;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _agent = GetComponent<NavMeshAgent>();
        }
        
        void Start()
        {
            _attackCollider.SetActive(false);
        }

        void Update()
        {
            // chase player
            if (Time.time >= _timeStamp + _timeDelay)
            {
                _agent.SetDestination(_player.transform.position);
                _timeStamp = Time.time;
                DetectPlayer();
            }
        }

        private void DetectPlayer()
        {
            float currentTargetDistance = Vector3.Distance(transform.position, _player.transform.position);
            if (currentTargetDistance <= _rangeOfAttack)
            {
                
                _agent.isStopped = true;
                if (Time.time > _nextAttack)
                {
                    _nextAttack = Time.time + _damageRate;
                    EnemyAttack();
                }
            }
            else
            {
                _agent.isStopped = false;
            }
        }

        private void EnemyAttack()
        {
            Collider[] hitPlayers = Physics.OverlapBox(_attackCollider.transform.position, _attackCollider.transform.position);
            foreach(Collider player in hitPlayers)
            {
                Debug.Log("Player attacked");
            }
        }
    }
}
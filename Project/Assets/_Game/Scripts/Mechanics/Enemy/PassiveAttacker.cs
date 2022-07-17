using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Mechanics.Enemy
{
    public class PassiveAttacker : EnemyBase
    {
        [Header("Melee")]
        [SerializeField]
        GameObject _attackCollider;

        [SerializeField]
        SOSpriteAnimation _transformAnimation;
        
        [SerializeField]
        SOSpriteAnimation _walkAnimation;

        [SerializeField]
        SOSpriteAnimation _attackAnimation;

        readonly float SEARCH_INTERVAL = 0.2f;

        NavMeshAgent _agent;
        bool _passive = true;
        float _stampForNextAttack;

        protected override void OnAwake()
        {
            base.OnAwake();
            _agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _attackCollider.SetActive(false);
        }
        
        public override void Harm(float damage)
        {
            base.Harm(damage);
            if (_passive)
            {
                _passive = false;
                _anim.LoadAnimation(_transformAnimation);
                StartCoroutine(WaitThen(_anim.Length, () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                    StartCoroutine(SeekLoop());
                }));
            }
        }

        IEnumerator SeekLoop()
        {
            _anim.LoadAnimation(_walkAnimation);
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

            _anim.LoadAnimation(_attackAnimation);
            StartCoroutine(WaitThen(_anim.Length, () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                }));
        }
    }
}

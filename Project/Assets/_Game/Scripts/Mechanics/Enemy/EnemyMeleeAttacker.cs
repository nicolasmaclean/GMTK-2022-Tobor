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
    public class EnemyMeleeAttacker : EnemyBase
    {
        [Header("Melee")]
        [SerializeField]
        GameObject _attackCollider;
        public UnityEvent OnAttack;

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
            _attackCollider.SetActive(false);
            StartCoroutine(SeekLoop());
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
                PlayerController hero = player.GetComponentInParent<PlayerController>();
                if(hero != null)
                {
                    hero.Hurt(_attack);
                    Debug.Log("Attacked Player");
                }
                
            }
            
            _anim.LoadAnimation(_attackAnimation);
            OnAttack?.Invoke();
            StartCoroutine(SwapToWalk(_anim.Length));
        }

        IEnumerator SwapToWalk(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _anim.LoadAnimation(_walkAnimation);
        }
    }
}
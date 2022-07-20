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

        PlayerTrigger _playerTrigger;
        NavMeshAgent _agent;
        float _stampForNextAttack;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _agent = GetComponent<NavMeshAgent>();
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            StartCoroutine(SeekLoop());
        }

        IEnumerator SeekLoop()
        {
            _anim.LoadAnimation(_walkAnimation);
            while (true)
            {
                NavMeshPath path = CalculatePath();
                
                NavMeshPathStatus status = path.status;
                if (status == NavMeshPathStatus.PathComplete)
                {
                    _agent.SetDestination(_player.transform.position);
                }
                else
                {
                    _agent.ResetPath();
                    DetectPlayer();
                }

                yield return new WaitForSeconds(SEARCH_INTERVAL);
            }
        }

        NavMeshPath CalculatePath()
        {
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(_player.transform.position, path);
            return path;
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
            if (_playerTrigger.PlayerIsIn)
            {
                PlayerController.Instance.Hurt(_attack);
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
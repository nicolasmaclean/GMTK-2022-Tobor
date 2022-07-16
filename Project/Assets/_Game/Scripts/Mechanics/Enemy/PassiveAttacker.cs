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
        SOSpriteAnimation _walkAnimation;

        [SerializeField]
        SOSpriteAnimation _attackAnimation;


        readonly float SEARCH_INTERVAL = 0.2f;

        AnimatedSprite _anim;
        NavMeshAgent _agent;
        float _stampForNextAttack;

        protected override void OnAwake()
        {
            base.OnAwake();
            _anim = transform.GetComponentInChildren<AnimatedSprite>();
            _agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _attackCollider.SetActive(false);
            if (isHarmed)
            {
                StartCoroutine(SeekLoop());
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
            StartCoroutine(SwapToWalk(_anim.Length));
        }

        IEnumerator SwapToWalk(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _anim.LoadAnimation(_walkAnimation);
        }
    }
}

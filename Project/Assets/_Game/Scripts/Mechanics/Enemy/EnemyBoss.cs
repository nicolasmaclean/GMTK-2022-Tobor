using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBoss : EnemyBase
    {
        [Header("Boss")]
        [SerializeField]
        GameObject _attackCollider;

        [SerializeField]
        SOSpriteAnimation _walkAnimation;

        [SerializeField]
        SOSpriteAnimation _attackAnimation;

        [SerializeField]
        SOSpriteAnimation _shootAnimation;

        [SerializeField] float _rangeOfShot = 6f;
        [SerializeField] Transform _bulletSpawnPoint;
        [SerializeField] GameObject _bullet;
        [SerializeField] float _fireRate = 3f;

        readonly float SEARCH_INTERVAL = 0.2f;

        NavMeshAgent _agent;
        float _stampForNextAttack;
        float _stampForNextShot;
        bool canShoot;
        

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
            canShoot = true;
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
            if (currentTargetDistance <= _rangeOfShot)
            {
                if (Time.time > _stampForNextShot)
                {
                    _stampForNextShot = Time.time + _fireRate;
                    EnemyShoot(canShoot);
                }
                if (currentTargetDistance <= _rangeOfAttack)
                {
                    _agent.isStopped = true;
                    canShoot = false;
                    if (Time.time > _stampForNextAttack)
                    {
                        _stampForNextAttack = Time.time + _damageRate;
                        EnemyAttack();
                    }
                }
                else
                {
                    canShoot = true;
                    _agent.isStopped = false;
                }
            }
        }

        void EnemyAttack()
        {
            Collider[] hitPlayers = Physics.OverlapBox(_attackCollider.transform.position, _attackCollider.transform.position);
            foreach (Collider player in hitPlayers)
            {
                PlayerController hero = player.GetComponentInParent<PlayerController>();
                if (hero != null)
                {
                    hero.Hurt(_attack);
                    Debug.Log("Attacked Player");
                }

            }

            _anim.LoadAnimation(_attackAnimation);
            StartCoroutine(SwapToWalk(_anim.Length));
        }

        void EnemyShoot(bool rangeCheck)
        {
            if (rangeCheck)
            {
                _anim.LoadAnimation(_shootAnimation);
                _anim.PlayOneShot(_shootAnimation, () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                });

                StartCoroutine(WaitThen(_anim.Spf * 3, () =>
                {
                    Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
                }
                ));
            }
        }

        IEnumerator SwapToWalk(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _anim.LoadAnimation(_walkAnimation);
        }
    }
}

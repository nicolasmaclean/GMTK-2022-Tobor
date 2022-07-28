using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using UnityEngine;
using UnityEngine.AI;
using Game.Mechanics.Player;
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
        EnemyAISensor sensor;

        [SerializeField]
        GameObject _bullet;

        [SerializeField]
        int _shootDelay = 3;

        protected override void OnStart()
        {
            base.OnStart();
            sensor = GetComponentInChildren<EnemyAISensor>();
        }
        protected override void EnemyAttack()
        {
            if (sensor.IsInSight(_player.gameObject))
            {
                Debug.Log("Found Human");
                _agent.isStopped = true;
                _anim.PlayOneShot(_attackAnimation, speedMultiplier: Modifiers.AttackSpeedMultiplier, callback: () =>
                {
                    _anim.LoadAnimation(_walkAnimation);
                    _agent.isStopped = false;
                });

                StartCoroutine(WaitThen((_anim.Spf / Modifiers.AttackSpeedMultiplier) * _shootDelay, () =>
                {
                    OnShoot?.Invoke();
                    EnemyBullet bullet = Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation).GetComponent<EnemyBullet>();
                    bullet._damage = _attack;
                }));
            }
        }

        
    }
}
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Events;


namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowBullet : MonoExtended
    {
        static float Damage
        {
            get
            {
                if (damage == -1)
                {
                    damage = PlayerStats.GetInRange(PlayerStats.Instance.Strength, PlayerStats.Instance.StrengthRange);
                }

                return damage;
            }
        }
        static float damage = -1f;
        
        [SerializeField]
        float _arrowSpeed = 100f;

        [SerializeField]
        float _lifeSpan = 20f;
        
        PlayerController _player;
        public UnityEvent OnHit;

        void Start()
        {
            _player = PlayerController.Instance;
            Destroy(gameObject, _lifeSpan);
        }

        void Update()
        {
            var t = transform;
            t.position += t.forward * (_arrowSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            EnemyBase em = other.GetComponentInParent<EnemyBase>();
            ShootButton btn = other.GetComponentInParent<ShootButton>();
            if (em)
            {
                Vector3 hitPosition = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (_player.transform.position - em.transform.position).normalized;
                em.Harm(Damage, hitPosition, hitNormal);
            }
            else if (btn)
            {
                btn.Activate();
            }
            
            Die();
        }

        void Die()
        {
            OnHit?.Invoke();
            Destroy(gameObject);
        }
    }
}

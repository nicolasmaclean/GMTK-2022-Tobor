using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBullet : MonoExtended
    {
        public float _damage = 1;
        
        [SerializeField]
        float _bulletSpeed;

        [SerializeField]
        float _lifeSpan = 20f;

        // [SerializeField]
        // float _acceleration;

        Transform sprite;
        float _timeAlive;

        public UnityEvent OnHit;

        void Awake()
        {
            sprite = transform.GetChild(0);
        }
        
        void Start()
        {
            LookAtPlayer(transform);
        }

        void FixedUpdate()
        {
            LookAtPlayer(sprite);
            var t = transform;
            t.position += t.forward * (_bulletSpeed * Time.fixedDeltaTime);
        }

        void Update()
        {
            if (_timeAlive > _lifeSpan)
            {
                Die();
            }
            
            _timeAlive += Time.deltaTime;
        }

        void LookAtPlayer(Transform from)
        {
            Vector3 playerPosition = Camera.main.transform.position;
            from.LookAt(playerPosition);
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
            
            if (player != null)
            {
                player.Hurt(_damage);
            }
            else
            {
                Transform parent = other.transform.parent;
                if (parent)
                {
                    EnemyBase em = parent.GetComponentInParent<EnemyBase>();
                    if (em) return;
                }
            }

            Die();
        }

        void Die()
        {

            OnHit?.Invoke();
            Destroy(gameObject, .01f);
        }
    }
}
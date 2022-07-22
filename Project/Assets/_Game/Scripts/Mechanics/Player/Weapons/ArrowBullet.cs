using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Events;


namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowBullet : MonoBehaviour
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
        float _arrowSpeed;

        [SerializeField]
        float _lifeSpan = 20f;
        
        PlayerController _player;
        float _timeAlive = 0;
        public UnityEvent OnHit;

        void Awake()
        {
            _player = PlayerController.Instance;
        }

        void Update()
        {
            var t = transform;
            t.position += t.forward * (_arrowSpeed * Time.deltaTime);
            
            if (_timeAlive > _lifeSpan)
            {
                Die();
            }
            
            _timeAlive += Time.deltaTime;
        }

        void OnTriggerEnter(Collider other)
        {
            Transform parent = other.transform.parent;
            if (!parent)
            {
                Destroy(gameObject, .01f);
                return;
            }
            
            EnemyBase em = parent.GetComponentInParent<EnemyBase>();
            if (!em)
            {
                Die();
                return;
            }
            
            em.Harm(Damage);
        }

        void Die()
        {
            OnHit?.Invoke();
            Destroy(gameObject, 0.01f);
        }

        public void PlaySFX(SOAudioClip clip)
        {
            SFXManager.PlaySFX(clip);
        }
    }
}

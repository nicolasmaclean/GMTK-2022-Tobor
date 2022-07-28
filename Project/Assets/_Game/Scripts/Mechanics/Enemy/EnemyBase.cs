using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Game.Mechanics.Enemy
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyBase : MonoExtended
    {
        public float Health { get; protected set; }
        public Action<EnemyBase> OnKilled;

        [Header("Stats")]
        [SerializeField]
        protected float _baseHealth = 10f;
        
        [SerializeField]
        protected float _attack = 1f;
        
        [SerializeField]
        protected float _attackSpeed = 2f;
        
        [SerializeField]
        protected float _range = 3f;

        [SerializeField]
        [Utility.ReadOnly]
        protected float _lastAttack;
        
        [Header("Events")]
        public UnityEvent<Vector3, Vector3> OnHurt;
        public UnityEvent <Vector3> OnDead;

        [Header("Animations")]
        [SerializeField]
        protected SOSpriteAnimation _walkAnimation; 

        protected NavMeshAgent _agent;
        protected PlayerController _player;
        protected AnimatedSprite _anim;
        protected float _rangeSqr;
        
        protected readonly float SEARCH_INTERVAL = 0.2f;


        void Awake()
        {
            Health = _baseHealth;
            _anim = transform.GetComponentInChildren<AnimatedSprite>();
            _agent = GetComponent<NavMeshAgent>();
            _rangeSqr = _range * _range;
            OnAwake();
        }
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            _rangeSqr = _range * _range;
        }
        #endif

        void Start()
        {
            _player = PlayerController.Instance;
            OnStart();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnStart()
        {
            StartCoroutine(SeekLoop());
        }

        protected virtual IEnumerator SeekLoop()
        {
            if (_walkAnimation)
            {
                _anim.LoadAnimationRandom(_walkAnimation);
            }
            
            while (true)
            {
                NavMeshPath path = CalculatePath();
                
                NavMeshPathStatus status = path.status;
                if (status == NavMeshPathStatus.PathComplete)
                {
                    _agent.SetDestination(_player.transform.position);
                    DetectPlayer();
                }
                else
                {
                    _agent.ResetPath();
                }

                yield return new WaitForSeconds(SEARCH_INTERVAL);
                _lastAttack += SEARCH_INTERVAL;
            }
        }

        protected virtual void DetectPlayer()
        {
            Vector3 dir = transform.position - _player.transform.position;
            float distSqr = dir.sqrMagnitude;
            if (distSqr <= _rangeSqr)
            {
                if (_lastAttack > _attackSpeed / Modifiers.AttackSpeedMultiplier)
                {
                    _lastAttack = 0;
                    EnemyAttack();
                }
            }
        }

        protected abstract void EnemyAttack();

        public virtual void Harm(float damage, Vector3 hitPosition, Vector3 hitNormal)
        {
            Health -= damage * Modifiers.DamageMultiplier;
            OnHurt?.Invoke(hitPosition, hitNormal);
            
            if (Health <= 0)
            {
                Kill();
            }
        }

        protected virtual void Kill()
        {
            OnDead?.Invoke(transform.position);
            OnKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
        
        protected static IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
        
        protected NavMeshPath CalculatePath()
        {
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(_player.transform.position, path);
            return path;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}

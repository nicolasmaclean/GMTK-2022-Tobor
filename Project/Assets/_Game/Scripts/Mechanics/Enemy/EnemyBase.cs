using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Game.Mechanics.Enemy
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyBase : MonoBehaviour
    {
        public float Health { get; protected set; }
        public Action<EnemyBase> OnKilled;

        [Header("Stats")]
        [SerializeField]
        protected float _baseHealth = 10f;
        
        [SerializeField]
        protected float _attack = 1f;
        
        [SerializeField]
        protected float _damageRate = 2f;
        
        [SerializeField]
        protected float _rangeOfAttack = 3f;

        [Header("Events")]
        public UnityEvent OnHurt;
        public UnityEvent OnDead;

        [Header("Animations")]
        [SerializeField]
        protected SOSpriteAnimation _walkAnimation; 

        protected NavMeshAgent _agent;
        protected PlayerController _player;
        protected AnimatedSprite _anim;
        protected bool isHarmed;
        
        protected readonly float SEARCH_INTERVAL = 0.2f;


        void Awake()
        {
            Health = _baseHealth;
            isHarmed = false;
            _anim = transform.GetComponentInChildren<AnimatedSprite>();
            _agent = GetComponent<NavMeshAgent>();
            OnAwake();
        }

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
            }
        }

        protected virtual void DetectPlayer()
        {
            // float currentTargetDistance = Vector3.Distance(transform.position, _player.transform.position);
            // if (currentTargetDistance <= _rangeOfAttack)
            // {
            //     _agent.isStopped = true;
            //     if (Time.time > _stampForNextAttack)
            //     {
            //         _stampForNextAttack = Time.time + _damageRate;
            //         EnemyAttack();
            //     }
            // }
            // else
            // {
            //     _agent.isStopped = false;
            // }
        }

        public virtual void Harm(float damage)
        {
            Health -= damage * Modifiers.DamageMultiplier;
            isHarmed = true;
            OnHurt?.Invoke();
            
            if (Health <= 0)
            {
                Kill();
            }
        }

        protected virtual void Kill()
        {
            OnDead?.Invoke();
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

        public void PlaySFX(SOAudioClip clip)
        {
            SFXManager.PlaySFX(clip);
        }
    }
}

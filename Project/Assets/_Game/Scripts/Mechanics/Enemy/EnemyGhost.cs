using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Mechanics.Player;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Mechanics.Enemy
{
    public class EnemyGhost : EnemyBase
    {   
        [SerializeField]
        SOSpriteAnimation _attackAnimation;

        [SerializeField]
        [Range(0, 1)]
        float _hiddenAlpha = .03f;

        [Header("More Stats!")]
        [SerializeField]
        float dashForce = 50;

        [SerializeField]
        float _dashLength = 1f;

        [SerializeField]
        float _dashCooldown = 3f;

        [Header("Colliders")]
        [SerializeField]
        Collider _enemyCollider;

        [Header("Debug")]
        [SerializeField]
        [Utility.ReadOnly]
        bool _dashing = false;

        PlayerTrigger _trigger;
        SpriteRenderer _renderer;
        Collider _collider;
        Rigidbody _rb;
        
        float _defaultAlpha;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            _trigger = GetComponentInChildren<PlayerTrigger>();
            _renderer = _anim.GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody>();
            
            _trigger.OnEnter.AddListener(HurtPlayer);
            _defaultAlpha = _renderer.color.a;
        }

        void OnDestroy()
        {
            _trigger.OnEnter.RemoveListener(HurtPlayer);
        }

        protected override void EnemyAttack() => Dash();

        void Dash()
        {
            _agent.isStopped = true;
            _anim.LoadAnimation(_attackAnimation);
            _renderer.color = new Color(1f,1f,1f,_defaultAlpha);
            
            Vector3 forceToApply = (PlayerController.Instance.transform.position - transform.position).normalized * dashForce;
            _rb.AddForce(forceToApply, ForceMode.Impulse);
            _dashing = true;

            StartCoroutine(Dashing());

            IEnumerator Dashing()
            {
                yield return new WaitForSeconds(_dashLength);
                
                _rb.velocity = Vector3.zero;
                _anim.LoadAnimation(_walkAnimation);
                _dashing = false;

                Color color = new Color(1f,1f,1f, _hiddenAlpha);
                _renderer.color = color;
                _rb.isKinematic = true;
                _enemyCollider.enabled = false;
                
                yield return new WaitForSeconds(_dashCooldown);

                color = new Color(1f,1f,1f,_defaultAlpha);
                _renderer.color = color;
                _rb.isKinematic = false;
                _enemyCollider.enabled = true;
            }
        }

        void HurtPlayer()
        {
            if (!_dashing) return;
            
            PlayerController.Instance.Hurt(_attack);
        }
    }
}


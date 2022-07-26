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
        
        [Header("More Events!")]
        [SerializeField]

        RaycastHit hit;
        Rigidbody rb;

        [SerializeField] float dashForce = 50;
        [SerializeField] float waitTillmove = 1f;
        [SerializeField] float dashCooldown;

        [SerializeField] SpriteRenderer SR;

        private void FixedUpdate() 
        {
            if(_lastAttack > dashCooldown)
            {
                rb.velocity = Vector3.zero;
                _anim.LoadAnimation(_walkAnimation);
            }

            if(_lastAttack > dashCooldown + waitTillmove)
            {
                _agent.isStopped = false;
                Hidden();
            }

            if(_lastAttack > 4)
            {
                NotHidden();
            }

        }

        protected override void OnAwake()
        {
            base.OnAwake();
            rb = GetComponent<Rigidbody>();
            Hidden();
        }

        protected override void EnemyAttack()
        {

            if (Physics.SphereCast(transform.position, 10, transform.forward, out hit, _range, ~LayerMask.NameToLayer("Player")))
            {
                _agent.isStopped = true;
                _anim.LoadAnimation(_attackAnimation);
                Dash();
            }
        }

        private void Dash()
        {
            Vector3 forceToApply = transform.forward * dashForce;
            rb.AddForce(forceToApply, ForceMode.Impulse);
            SR.color = new Color(1f,1f,1f,.8f);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player")
            {
                PlayerController.Instance.Hurt(_attack);
            }
        }

        private void Hidden()
        {
            SR.color = new Color(1f,1f,1f,.03f);
            rb.isKinematic = true;
            GetComponentInChildren<Collider>().enabled = false;
        }

        private void NotHidden()
        {
            SR.color = new Color(1f,1f,1f,.8f);
            rb.isKinematic = false;
            GetComponentInChildren<Collider>().enabled = true;
        }
    }
}


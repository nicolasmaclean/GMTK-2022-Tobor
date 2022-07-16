using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] float _bulletSpeed;
        [SerializeField] Rigidbody rb;



        private void OnTriggerEnter(Collider other)
        {
            CharacterController player = other.gameObject.GetComponentInParent<CharacterController>();
            if (player != null)
            {
                Debug.Log("Player Shot");
            }
            Destroy(gameObject, .01f);
        }
        void Awake()
        {
           rb = gameObject.GetComponent<Rigidbody>();
        }
        
        void Start()
        {
           rb.velocity = transform.forward * _bulletSpeed;
        }
    }
}
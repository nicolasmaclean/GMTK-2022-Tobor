using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] float _bulletSpeed;

        void Awake()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
        }
        
        void Start()
        {
           // rb.velocity = Vector3.forward * _bulletSpeed;
        }

        void Update()
        {
            
        }
    }
}
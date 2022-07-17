using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField]
        float _bulletSpeed;

        [SerializeField]
        float _scaleFactor = .2f;
        
        [SerializeField]
        float _scaleFrequency = .2f;

        Rigidbody _rb;
        Transform sprite;
        
        void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
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
            sprite.localScale = Vector3.one + Vector3.one * Mathf.Sin(_scaleFrequency * Time.time) * _scaleFactor;
        }

        void LookAtPlayer(Transform from)
        {
            Vector3 playerPosition = Camera.main.transform.position;
            from.LookAt(playerPosition);
        }
        
        void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
            
            if (player != null)
            {
                
                Debug.Log("Player Shot");
                Destroy(gameObject, .01f);
            }
        }
    }
}
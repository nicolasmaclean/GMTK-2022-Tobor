using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;


namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowBullet : MonoBehaviour
    {
        [SerializeField] float _arrowSpeed;
        [SerializeField] Rigidbody rb;
        PlayerController _player;

        void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            _player = PlayerController.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyBase monster = other.gameObject.GetComponentInParent<EnemyBase>();
            if (monster != null)
            {
                monster.Harm((int)_player.Weapon.Damage);
            Debug.Log("Shot Enemy");
            }
            Destroy(gameObject, .01f);
        }
        

        void Start()
        {
            rb.velocity = transform.forward * _arrowSpeed;
        }
    }
}

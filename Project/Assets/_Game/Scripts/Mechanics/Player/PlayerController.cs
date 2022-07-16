using System;
using Game.Utility;
using UnityEngine;
using Game.Mechanics;

namespace Game.Mechanics.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
        public float LastAttackTime = float.MaxValue;
        
        public SOWeapon Weapon
        {
            get
            {
                switch (_CurrentWeapon)
                {
                    default:
                    case WeaponType.Sword:
                        return Sword;
                }
            }
        }
        
        [Header("Data")]
        public PlayerStats _stats;
        public SOWeapon Sword;
        public SOWeapon Bow;

        [Header("State")]
        [SerializeField]
        WeaponType _CurrentWeapon;
        
        [Header("Controls")]
        [SerializeField]
        KeyCode _primaryKey = KeyCode.Mouse0;

        FPSController _playerController;
        Animator _animator;
        

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            _playerController = GetComponent<FPSController>();
            _animator = GetComponent<Animator>();
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        void Start()
        {
            _playerController.UpdateSpeed(_stats.Agility / 20f);
            ChangeWeapon(_CurrentWeapon);
        }

        void Update()
        {
            LastAttackTime += Time.deltaTime;
            if (LastAttackTime < Weapon.Cooldown) return;
            
            if (Input.GetKeyDown(_primaryKey))
            {
                Attack();
            }
        }

        void Attack()
        {
            _animator.SetTrigger(AT_SWORD_PRIMARY);
            LastAttackTime = 0;
        }

        void ChangeWeapon(WeaponType weaponType)
        {
            SOWeapon weapon;
            switch (weaponType)
            {
                default:
                case WeaponType.Sword:
                    _CurrentWeapon = WeaponType.Sword;
                    weapon = Sword;
                    _animator.SetTrigger(AT_SWORD_DRAW);
                    break;
            }

            _animator.speed = weapon.Speed;
        }

        readonly String AT_SWORD_DRAW = "Sword_Draw";
        readonly String AT_SWORD_PRIMARY = "Sword_Primary";
    }
}
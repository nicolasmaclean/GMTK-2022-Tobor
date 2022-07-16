using System;
using Game.Utility;
using UnityEngine;
using Game.Mechanics;

namespace Game.Mechanics.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
        public static PlayerStats Stats;
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
                    case WeaponType.Bow:
                        return Bow;
                }
            }
        }
        
        [Header("Data")]
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
        [SerializeField] GameObject _arrow;
        [SerializeField] Transform _arrowSpawnPoint;
        

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

            Stats ??= PlayerStats.CreateRandom();
            
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
            _playerController.UpdateSpeed(Stats.Agility / 20f);
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
            if (_CurrentWeapon == WeaponType.Sword)
            {
                _animator.SetTrigger(AT_SWORD_PRIMARY);
            }
            else if (_CurrentWeapon == WeaponType.Bow)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = ray.GetPoint(75);
                }
                Vector3 direction = targetPoint - _arrowSpawnPoint.transform.position;
                GameObject currentBullet = Instantiate(_arrow, _arrowSpawnPoint.transform.position, _arrowSpawnPoint.transform.rotation);
                currentBullet.transform.forward = direction.normalized;
                
            }

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
                case WeaponType.Bow:
                    _CurrentWeapon = WeaponType.Bow;
                    weapon = Bow;
                    break;
            }

            _animator.speed = weapon.Speed;
        }

        readonly String AT_SWORD_DRAW = "Sword_Draw";
        readonly String AT_SWORD_PRIMARY = "Sword_Primary";
    }
}
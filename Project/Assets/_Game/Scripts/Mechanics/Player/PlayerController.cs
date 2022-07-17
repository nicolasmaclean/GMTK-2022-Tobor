using System;
using System.Collections;
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

        [SerializeField]
        KeyCode _secondaryKey = KeyCode.Mouse1;

        [Header("Bow")]
        [SerializeField]
        GameObject PF_Arrow;
        
        [SerializeField]
        Transform _arrowSpawn;
        
        FPSController _playerController;
        Animator _animator;
        RaycastHit hit;

        #region MonoBehaviour
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
                PrimaryAttack();
            }
            else if (Input.GetKeyDown(_secondaryKey))
            {
                SecondaryAttack();
            }
        }
        #endregion

        void PrimaryAttack()
        {
            if (_CurrentWeapon == WeaponType.Sword)
            {
                PrimaryAttackSword();
            }
            else if (_CurrentWeapon == WeaponType.Bow)
            {
                PrimaryAttackBow();
            }

            LastAttackTime = 0;
        }

        void PrimaryAttackSword()
        {
            _animator.SetTrigger(AT_SWORD_PRIMARY);
        }

        void PrimaryAttackBow()
        {
            _animator.SetTrigger(AT_BOW_FIRE);
            StartCoroutine(WaitThen(.12f , () =>
            {
                // ray from center of screen going forwards
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                
                // shoot towards immediate object or directly forwards toward the horizon
                Vector3 targetPoint;
                targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(HORIZON_DISTANCE);
                
                Vector3 direction = targetPoint - _arrowSpawn.transform.position;
                GameObject currentBullet = Instantiate(PF_Arrow, _arrowSpawn.transform.position, _arrowSpawn.transform.rotation);
                currentBullet.transform.forward = direction.normalized;
            }));
        }

        void SecondaryAttack()
        {
            // if (_CurrentWeapon == WeaponType.Sword)
            // {
            //     
            // }
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
                    _animator.SetTrigger(AT_BOW_DRAW);
                    break;
            }

            _animator.speed = weapon.Speed;
        }
        
        static IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }

        readonly float HORIZON_DISTANCE = 75;

        readonly String AT_SWORD_DRAW      = "Sword_Draw";
        readonly String AT_SWORD_PRIMARY   = "Sword_Primary";
        // readonly String AT_SWORD_SECONDARY = "Sword_Secondary";
        
        readonly String AT_BOW_DRAW   = "Bow_Draw";
        readonly String AT_BOW_FIRE   = "Bow_Fire";
    }
}
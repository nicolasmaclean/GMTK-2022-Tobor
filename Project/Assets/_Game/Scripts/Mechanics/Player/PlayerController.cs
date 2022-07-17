using System;
using System.Collections;
using Game.Utility;
using UnityEngine.UI;
using Game.Mechanics.Enemy;
using UnityEngine;
using Game.Mechanics;

namespace Game.Mechanics.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Public
        public static PlayerController Instance { get; private set; }
        
        public float LastAttackTime { get; private set; } = float.MaxValue;
        public float Health
        {
            get
            {
                return _health;
            }
        }
        
        [Header("UI")]
        [SerializeField] Image hurtScreen;
        [SerializeField] Color hurt;
        [SerializeField] Color fine;
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject crosshairs;
        [SerializeField] Image crosshairsColor;
        [SerializeField] GameObject winMenu;
        [SerializeField] GameObject loseMenu;

        public SOWeapon Weapon
        {
            get
            {
                switch (_currentWeapon)
                {
                    default:
                    case WeaponType.Sword:
                        return Sword;
                    case WeaponType.Bow:
                        return Bow;
                }
            }
        }
        #endregion

        #region Serialized or Private
        [Header("State")]
        [SerializeField]
        [ReadOnly]
        float _health = 0;

        [SerializeField]
        WeaponType _currentWeapon;
        
        [Header("Controls")]
        [SerializeField]
        KeyCode _primaryKey = KeyCode.Mouse0;

        [SerializeField]
        KeyCode _secondaryKey = KeyCode.Mouse1;

        [SerializeField]
        KeyCode _switchKey = KeyCode.Q;
        
        [Header("Sword")]
        public SOWeapon Sword;

        [SerializeField]
        [Min(0)]
        float _maxTimeInChain = 1f;

        [Header("Bow")]
        public SOWeapon Bow;
        
        [SerializeField]
        GameObject PF_Arrow;
        
        [SerializeField]
        Transform _arrowSpawn;
        
        Animator _animator;
        RaycastHit _hitinfo;
        int _curSwing = 0;
        float _lastSwing = 0;
        #endregion

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
            ChangeWeapon(_currentWeapon);
            SetHealth();
        }

        void Update()
        {
            LastAttackTime += Time.deltaTime;
            if (LastAttackTime < Weapon.Cooldown) return;
            EnemyTargeted();
            
            if (Input.GetKeyDown(_primaryKey))
            {
                PrimaryAttack();
            }
            else if (Input.GetKeyDown(_secondaryKey))
            {
                SecondaryAttack();
            }
            else if (Input.GetKeyDown(_switchKey))
            {
                SwitchWeapons();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        #endregion

        #region UI
        void PauseGame()
        {
            StopGame();
            pauseMenu.SetActive(true);
        }

        void WinGame()
        {
            StopGame();
            winMenu.SetActive(true);
        }

        void LoseGame()
        {
            StopGame();
            loseMenu.SetActive(true);
        }

        void StopGame()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Time.timeScale = 0;
            
            crosshairs.SetActive(false);
        }

        void EnemyTargeted()
        {

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                EnemyBase enemy = hit.transform.gameObject.GetComponentInParent<EnemyBase>();
                if(enemy != null)
                {
                    crosshairsColor.color = Color.red;
                }
                else
                {
                    crosshairsColor.color = Color.white;
                }
            }
            
        }
        #endregion

        #region Stats
        void SetHealth()
        {
            _health = PlayerStats.GetInRange(PlayerStats.Instance.Constitution, PlayerStats.Instance.ConstitutionRange);
        }
        #endregion
        
        void PrimaryAttack()
        {
            if (_currentWeapon == WeaponType.Sword)
            {
                PrimaryAttackSword();
            }
            else if (_currentWeapon == WeaponType.Bow)
            {
                PrimaryAttackBow();
            }

            LastAttackTime = 0;
        }

        void PrimaryAttackSword()
        {
            // reset chain
            if (Time.time - _lastSwing >= _maxTimeInChain || _curSwing > 1)
            {
                _curSwing = 0;
            }

            LastAttackTime = _curSwing == 0 ? .33f : .55f;
            _lastSwing = Time.time;
            _animator.SetTrigger(AT_SWORD_PRIMARY_ + (_curSwing + 1).ToString());
            _curSwing++;
        }

        void PrimaryAttackBow()
        {
            _animator.SetTrigger(AT_BOW_FIRE);
            StartCoroutine(WaitThen(.12f , () =>
            {
                // target is horizon from center of screen
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                Vector3 targetPoint = ray.GetPoint(HORIZON_DISTANCE);

                var pos = _arrowSpawn.transform.position;
                GameObject currentBullet = Instantiate(PF_Arrow, pos, Quaternion.identity);
                currentBullet.transform.forward = targetPoint - pos;
            }));
        }

        void SecondaryAttack()
        {
            // if (_CurrentWeapon == WeaponType.Sword)
            // {
            //     
            // }
        }

        void SwitchWeapons()
        {
            ChangeWeapon(_currentWeapon == WeaponType.Sword ? WeaponType.Bow : WeaponType.Sword);
        }

        void ChangeWeapon(WeaponType weaponType)
        {
            SOWeapon weapon;
            switch (weaponType)
            {
                default:
                case WeaponType.Sword:
                    weapon = Sword;
                    _animator.SetTrigger(AT_SWORD_DRAW);
                    break;
                
                case WeaponType.Bow:
                    weapon = Bow;
                    _animator.SetTrigger(AT_BOW_DRAW);
                    break;
            }

            _currentWeapon = weaponType;
            _animator.speed = weapon.Speed;
        }
        
        static IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }

        public void Hurt(float damage)
        {
            _health -= damage * Modifiers.DamageMultiplier;
            if (_health > 0)
            {
                StartCoroutine(dispayHurtScreen(hurtScreen, fine, hurt, .3f));
            }
            else
            {
                LoseGame();
            }
        }

        static IEnumerator dispayHurtScreen(Graphic hurtScreen, Color from, Color to, float seconds)
        {
            float startTime = Time.time;
            float TimeSinceStarted = Time.time - startTime;
            float percentageComplete = TimeSinceStarted / seconds;
            
            while (true)
            {
                TimeSinceStarted = Time.time - startTime;
                percentageComplete = TimeSinceStarted / seconds;
                hurtScreen.color = Color.Lerp(from, to, percentageComplete);
                if (percentageComplete >= 1) break;
                yield return new WaitForEndOfFrame();
            }
            
            float reverseStartTime = Time.time;
            float reverseTimeSinceStarted = Time.time - reverseStartTime;
            float reversePercentageComplete = reverseTimeSinceStarted / seconds;
            
            while (true)
            {
                reverseTimeSinceStarted = Time.time - reverseStartTime;
                reversePercentageComplete = reverseTimeSinceStarted / seconds;
                hurtScreen.color = Color.Lerp(to, from, reversePercentageComplete);
                if (reversePercentageComplete >= 1) break;
                yield return new WaitForEndOfFrame();
            }
        }
       
        readonly float HORIZON_DISTANCE = 75;

        readonly String AT_SWORD_DRAW      = "Sword_Draw";
        readonly String AT_SWORD_PRIMARY_   = "Sword_Primary_";
        
        readonly String AT_BOW_DRAW   = "Bow_Draw";
        readonly String AT_BOW_FIRE   = "Bow_Fire";
    }
}
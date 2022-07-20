using System;
using Game.Core;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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

        // [SerializeField] GameObject pauseMenu;
        // [SerializeField] GameObject winMenu;
        // [SerializeField] GameObject loseMenu;

        [FormerlySerializedAs("OnAttack"),Header("Events")]
        public UnityEvent OnSwordAttack;
        
        [FormerlySerializedAs("OnShoot")]
        public UnityEvent OnBowAttack;
        
        public UnityEvent OnHurt;
        public UnityEvent OnDeath;
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
        KeyCode _swordAttackKey = KeyCode.Mouse0;

        [SerializeField]
        KeyCode _bowAttackKey = KeyCode.Mouse1;

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
            if (Instance != this) return;
            
            Instance = null;
        }

        void Start()
        {
            SetHealth();
        }

        void Update()
        {
            LastAttackTime += Time.deltaTime;
            // if (LastAttackTime < Weapon.Cooldown) return;
            
            if (Input.GetKeyDown(_swordAttackKey))
            {
                SwordAttack();
            }
            else if (Input.GetKeyDown(_bowAttackKey))
            {
                BowAttack();
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
            // StopGame();
            // pauseMenu.SetActive(true);
        }
        
        public void WinGame()
        {
            // StopGame();
            // winMenu.SetActive(true);
        }
        
        void LoseGame()
        {
            // StopGame();
            // loseMenu.SetActive(true);
        }
        
        void StopGame()
        {
            // Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = true;
            //
            // Time.timeScale = 0;
        }
        #endregion

        #region Stats
        void SetHealth()
        {
            _health = PlayerStats.GetInRange(PlayerStats.Instance.Constitution, PlayerStats.Instance.ConstitutionRange);
        }
        #endregion
        
        #region Weapons
        void SwordAttack()
        {
            // reset chain
            if (Time.time - _lastSwing >= _maxTimeInChain || _curSwing > 1)
            {
                _curSwing = 0;
            }

            LastAttackTime = _curSwing == 0 ? .33f : .55f;
            _lastSwing = Time.time;
            _animator.SetTrigger(AT_SWORD_PRIMARY_ + (_curSwing + 1).ToString());
            OnSwordAttack?.Invoke();
            _curSwing++;
        }

        void BowAttack()
        {
            // play animation
            _animator.SetTrigger(AT_BOW_FIRE);
            
            // trigger event
            OnBowAttack?.Invoke();
            
            // fire projectile
            StartCoroutine(Coroutines.WaitThen(.12f , () =>
            {
                // target is horizon from center of screen
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                Vector3 targetPoint = ray.GetPoint(HORIZON_DISTANCE);

                var pos = _arrowSpawn.transform.position;
                GameObject currentBullet = Instantiate(PF_Arrow, pos, Quaternion.identity);
                currentBullet.transform.forward = targetPoint - pos;
            }));
        }
        #endregion
        
        public void Hurt(float damage)
        {
            _health -= damage * Modifiers.DamageMultiplier;

            if (_health < 0)
            {
                OnDeath?.Invoke();
                LoseGame();
            }
            else
            {
                OnHurt?.Invoke();
            }
        }
        
        #region Inspector Utilities
        public void PlaySFX(SOAudioClip clip)
        {
            SFXManager.PlaySFX(clip);
        }
        #endregion

        readonly float HORIZON_DISTANCE = 75;

        #region Animator Constants
        readonly String AT_SWORD_PRIMARY_   = "Sword_Primary_";
        readonly String AT_BOW_FIRE   = "Bow_Fire";
        #endregion
    }
}
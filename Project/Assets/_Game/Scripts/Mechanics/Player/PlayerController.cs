using System;
using System.Linq;
using Game.Core;
using Game.Mechanics.Level;
using Game.UI;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Mechanics.Player
{
    public class PlayerController : MonoExtended
    {
        #region Public
        public static PlayerController Instance { get; private set; }

        public UnityEvent<float> OnHealthChange;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = Mathf.Clamp(value, 0, _maxHealth);
            }
        }
        
        [FormerlySerializedAs("OnSwordAttack"),FormerlySerializedAs("OnAttack"),Header("Events")]
        public UnityEvent OnSwordSwing;
        
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
        [ReadOnly]
        float _maxHealth;

        [Header("Controls")]
        [SerializeField]
        KeyCode _swordAttackKey = KeyCode.Mouse0;

        [SerializeField]
        KeyCode _bowAttackKey = KeyCode.Mouse1;

        [SerializeField]
        public KeyCode InteractKey = KeyCode.E;

        [Header("Sword")]
        [SerializeField]
        Animator _swordAnimator;
        
        public SOWeapon Sword;

        [SerializeField]
        [Tooltip("Seconds before swing end that the next attack can be queued up.")]
        float _earlySwing = .1f; 

        [Header("Bow")]
        [SerializeField]
        Animator _bowAnimator;
        
        public SOWeapon Bow;
        
        [SerializeField]
        Transform _arrowSpawn;
        
        [SerializeField]
        GameObject PF_Arrow;

        [SerializeField]
        float _pickupDelay = 1f;

        Collider _swordCollider;
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

            _swordCollider = GetComponentInChildren<SwordCollision>(true).GetComponent<Collider>();

            _maxHealth = PlayerStats.Instance.ConstitutionRange.y;
            SetHealth();
        }

        void Start()
        {
            Modifiers.OnChange += ApplyModifiers;
        }

        void OnDestroy()
        {
            if (Instance != this) return;
            
            Instance = null;
            Modifiers.OnChange -= ApplyModifiers;
        }

        void Update()
        {
            if (Input.GetKeyDown(_swordAttackKey))
            {
                SwordAttack();
            }
            else if (Input.GetKeyDown(_bowAttackKey))
            {
                BowAttack();
            }
        }
        #endregion

        #region Stats
        void SetHealth()
        {
            Health = PlayerStats.GetInRange(PlayerStats.Instance.Constitution, PlayerStats.Instance.ConstitutionRange);
        }

        void ApplyModifiers()
        {
            _swordAnimator.speed = Modifiers.AttackSpeedMultiplier;
            _bowAnimator.speed   = Modifiers.AttackSpeedMultiplier;

            float oldHealth = Health;
            Health += Heal(Modifiers.Heal);
            if (Math.Abs(oldHealth - Health) > .01f)
            {
                OnHealthChange?.Invoke(Health);
            }
        }

        float Heal(float amount)
        {
            float scale = PlayerStats.Instance.ConstitutionRange.y / 5;
            return amount == 0 ? 0 : scale * Mathf.Pow(2, amount - 1); // (max / 10) * 2^(x-1) / 2
        }
        #endregion
        
        #region Weapons
        void SwordAttack()
        {
            if (_swordAnimator.IsInTransition(0)) return;

            AnimatorStateInfo state = _swordAnimator.GetCurrentAnimatorStateInfo(0);
            String trigger_swing;
            if (state.IsName("Swing 1") || state.IsName("Swing 2"))
            {
                float normalizedCooldown = (state.length - _earlySwing) / state.length;
                if (state.normalizedTime < normalizedCooldown)
                {
                    return;
                }

                trigger_swing = AT_SWORD_ATTACK + (state.IsName("Swing 1") ? "2" : "3");
            }
            else if (state.IsName("Idle"))
            {
                float timeInIdle = state.normalizedTime * state.length;
                if (timeInIdle < Sword.Cooldown)
                {
                    return;
                }

                trigger_swing = AT_SWORD_ATTACK + "1";
            }
            else
            {
                return;
            }
            
            
            if (state.IsName("Idle"))
            {
                for (int i = 0; i < 3; i++)
                {
                    _swordAnimator.ResetTrigger(AT_SWORD_ATTACK + (i+1).ToString());
                }
            }

            _swordAnimator.SetTrigger(trigger_swing);
        }
        
        public void SwordSwing() => OnSwordSwing?.Invoke();

        void BowAttack()
        {
            if (_bowAnimator.IsInTransition(0)) return;
            AnimatorStateInfo state = _bowAnimator.GetCurrentAnimatorStateInfo(0);

            if (!state.IsName("Idle")) return;
            
            float timeInIdle = state.normalizedTime * state.length;
            if (timeInIdle < Bow.Cooldown) return;
            
            _bowAnimator.SetTrigger(AT_BOW_FIRE);
            OnBowAttack?.Invoke();
        }

        public void FireArrow()
        {
            // target is horizon from center of screen
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            Vector3 targetPoint = ray.GetPoint(HORIZON_DISTANCE);

            var pos = _arrowSpawn.transform.position;
            GameObject currentBullet = Instantiate(PF_Arrow, pos, Quaternion.identity);
            currentBullet.transform.forward = targetPoint - pos;
        }

        public void PlayRollAnimation()
        {
            _bowAnimator.SetTrigger(AT_BOW_ROLL_ST);
            _bowAnimator.ResetTrigger(AT_BOW_ROLL_END);
        }

        public void FinishRollAnimation()
        {
            _bowAnimator.SetTrigger(AT_BOW_ROLL_END);
        }

        public void PlayBrandAnimation()
        {
            _bowAnimator.SetTrigger(AT_BOW_BRAND);
        }

        public void PlayPickupAnimation()
        {
            _bowAnimator.SetBool(AT_BOW_PICK, true);
            
            StartCoroutine(Coroutines.WaitThen(_pickupDelay, () =>
            {
                _swordAnimator.SetBool(AT_SWORD_PICK, true);
            }));
        }
        #endregion

        #region DEATH
        public void Hurt(float damage)
        {
            Health -= damage * Modifiers.DamageMultiplier;

            if (Health <= 0)
            {
                Kill();
            }
            else
            {
                OnHurt?.Invoke();
                OnHealthChange?.Invoke(Health);
            }
        }

        public void Kill()
        {
            OnDeath?.Invoke();
            GameMenuController.Lose();
        }
        #endregion
        
        #region Constants
        
        const float HORIZON_DISTANCE = 75;
        
        const string AT_SWORD_ATTACK        = "Attack_";
        static readonly int AT_SWORD_PICK   = Animator.StringToHash("Pickup");
        
        static readonly int AT_BOW_FIRE     = Animator.StringToHash("Fire");
        static readonly int AT_BOW_ROLL_ST  = Animator.StringToHash("RollStart");
        static readonly int AT_BOW_ROLL_END = Animator.StringToHash("RollEnd");
        static readonly int AT_BOW_BRAND    = Animator.StringToHash("Branded");
        static readonly int AT_BOW_PICK     = Animator.StringToHash("Pickup");
        #endregion
    }
}
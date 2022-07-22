using System;
using System.Linq;
using Game.Core;
using Game.UI;
using Game.Utility;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Mechanics.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Public
        public static PlayerController Instance { get; private set; }
        
        public float Health
        {
            get
            {
                return _health;
            }
        }

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

        [Header("Controls")]
        [SerializeField]
        KeyCode _swordAttackKey = KeyCode.Mouse0;

        [SerializeField]
        KeyCode _bowAttackKey = KeyCode.Mouse1;

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
            _health = PlayerStats.GetInRange(PlayerStats.Instance.Constitution, PlayerStats.Instance.ConstitutionRange);
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
            OnSwordAttack?.Invoke();
        }

        void BowAttack()
        {
            if (_bowAnimator.IsInTransition(0)) return;
            AnimatorStateInfo state = _bowAnimator.GetCurrentAnimatorStateInfo(0);

            if (!state.IsName("Idle")) return;
            
            float timeInIdle = state.normalizedTime * state.length;
            if (timeInIdle < Bow.Cooldown) return;
            
            _bowAnimator.SetTrigger(AT_BOW_FIRE);
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
                GameMenuController.Lose();
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
        readonly String AT_SWORD_ATTACK = "Attack_";
        readonly String AT_BOW_FIRE     = "Fire";
        // readonly String AT_BOW_ROLL     = "Roll";
        #endregion
    }
}
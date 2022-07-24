using System;
using System.Collections;
using Game.Mechanics.Player;
using Game.Utility;
using Game.Utility.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class InteractPrompt : Singleton<InteractPrompt>
    {
        public event Action OnReset;
        public bool Pressed
        {
            get
            {
                return _pressed;
            }
            set
            {
                _pressed = value;
                if (value)
                {
                    _down = false;
                    _currentTime = 0;
                    _idleTarget.position = _initialPosition;
                    PlayerController.Instance.PlayRollAnimation();
                }
                else
                {
                    PlayerController.Instance.FinishRollAnimation();
                }
            }
        }
        bool _pressed = false;
        
        [Header("Hold")]
        [SerializeField]
        Image _holdBar;

        [FormerlySerializedAs("_cycleLength"),SerializeField]
        float _fillCycleLength;

        [Header("In Animation")]
        [SerializeField]
        AnimationCurve _inCurve = AnimationCurve.Linear(0f, 0f, .3f, 1f);
        
        [Header("Idle Animation")]
        [SerializeField]
        Transform _idleTarget;

        [SerializeField]
        int _idleOffset = -10;

        [SerializeField]
        float _idleCycleLength = .7f;

        [SerializeField]
        [ReadOnly]
        float _currentTime;

        Vector3 _initialPosition;

        void Start()
        {
            Disable();
            _initialPosition = _idleTarget.position;
        }
        
        // need to integrate with HudController

        public void Enable()
        {
            _down = false;
            this.enabled = true;
            _currentTime = 0;
            _holdBar.fillAmount = 0;
            _idleTarget.position = _initialPosition;
            gameObject.SetActive(true);
            Pressed = false;
        }

        public void Disable()
        {
            _holdBar.fillAmount = 0;
            this.enabled = false;
            gameObject.SetActive(false);
            Pressed = false;
        }

        void Update()
        {
            if (Pressed)
            {
                UpdateFill();
            }
            else
            {
                UpdateIdle();
            }
        }

        void UpdateFill()
        {
            if (_currentTime > _fillCycleLength)
            {
                OnReset?.Invoke();
                _currentTime %= _fillCycleLength;
            }

            _holdBar.fillAmount = _currentTime / _fillCycleLength;
            _currentTime += Time.deltaTime;
        }

        bool _down = false;
        void UpdateIdle()
        {
            if (_currentTime > _idleCycleLength)
            {
                Vector3 pos = _initialPosition;
                if (!_down)
                {
                    pos.y += _idleOffset;
                }

                _idleTarget.position = pos;

                _down = !_down;
                _currentTime %= _idleCycleLength;   
            }

            _currentTime += Time.deltaTime;
        }

        public void Show()
        {
            Enable();
            StartCoroutine(Tween.UseCurve(_inCurve, (value) =>
            {
                var t = transform;
                Vector3 scal = t.localScale;
                
                scal.y = value;
                
                t.localScale = scal;
            }));
        }

        public void Hide()
        {
            Disable();
            // scale out (or fade) animation (use coroutine)
        }

        public void Reset()
        {
            _holdBar.fillAmount = 0;
            _currentTime = 0;
            Pressed = false;
        }
    }
}
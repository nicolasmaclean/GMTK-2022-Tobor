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
    public class ClickPrompt : Singleton<ClickPrompt>
    {
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
        
        public void Enable()
        {
            _down = false;
            this.enabled = true;
            _currentTime = 0;
            _idleTarget.position = _initialPosition;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            this.enabled = false;
            gameObject.SetActive(false);
        }

        void Update()
        {
            UpdateIdle();
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

        public void Hide() => Disable();

        public void Reset()
        {
            _currentTime = 0;
        }
    }
}
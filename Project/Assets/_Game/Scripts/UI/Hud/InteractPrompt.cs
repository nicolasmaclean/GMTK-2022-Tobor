using System;
using Game.Mechanics.Player;
using Game.Utility;
using UnityEngine;
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
                    PlayerController.Instance.PlayRollAnimation();
                }
                else
                {
                    PlayerController.Instance.FinishRollAnimation();
                }
            }
        }
        bool _pressed = false;
        
        [SerializeField]
        Image _holdBar;

        [SerializeField]
        float _cycleLength;

        [SerializeField]
        [ReadOnly]
        float _currentTime;

        void Start()
        {
            Disable();
        }
        
        // need to integrate with HudController

        public void Enable()
        {
            this.enabled = true;
            _currentTime = 0;
            _holdBar.fillAmount = 0;
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
            if (!Pressed) return;
            
            if (_currentTime > _cycleLength)
            {
                OnReset?.Invoke();
                _currentTime %= _cycleLength;
            }

            _holdBar.fillAmount = _currentTime / _cycleLength;
            _currentTime += Time.deltaTime;
        }

        public void Show()
        {
            Enable();
            // scale in animation (use coroutine)
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
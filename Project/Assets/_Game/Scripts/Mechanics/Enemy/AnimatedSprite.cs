using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Mechanics.Player;
using UnityEngine;
using Random = System.Random;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatedSprite : MonoBehaviour
    {
        public float Length
        {
            get
            {
                return _animation.Frames.Length * Spf;
            }
        }

        public float Spf { get; private set; }

        [SerializeField]
        SOSpriteAnimation _animation;

        [SerializeField]
        [Utility.ReadOnly]
        int _currentFrame;
        
        [SerializeField]
        [Utility.ReadOnly]
        float _speedMultiplier = 1;
        
        SpriteRenderer _renderer;
        float _timer = 0;
        Action callback = null;
        
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            LoadAnimation();
        }

        void OnValidate()
        {
            if (!_renderer) _renderer = GetComponent<SpriteRenderer>();
            if (!_animation)
            {
                _renderer.sprite = null;
                return;
            }
            LoadAnimation();
        }

        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > Spf / _speedMultiplier)
            {
                NextFrame();
                _timer = 0;
            }

            // apply billboard rotation
            Vector3 playerPosition = FPSController.Instance.transform.position;
            playerPosition.y = transform.position.y;
            transform.LookAt(playerPosition);
        }

        void NextFrame()
        {
            _currentFrame++;
            if (_currentFrame >= _animation.Frames.Length)
            {
                _currentFrame = 0;
                if (callback != null)
                {
                    Spf = float.PositiveInfinity;
                    callback();
                    callback = null;
                }
            }
            _renderer.sprite = _animation.Frames[_currentFrame];
        }

        public void PlayOneShot(SOSpriteAnimation data, Action callback, float speedMultiplier = 1)
        {
            LoadAnimation(data, speedMultiplier);
            this.callback = callback;
        }
        
        public void LoadAnimation(SOSpriteAnimation data, float speedMultiplier = 1)
        {
            if (data == _animation && !float.IsPositiveInfinity(Spf)) return;
            _animation = data;
            LoadAnimation();
            _speedMultiplier = speedMultiplier;
        }
        
        void LoadAnimation()
        {
            Spf = 1f / _animation.Fps;
            _currentFrame = _animation.InitialFrame;
            _renderer.sprite = _animation.Frames[_currentFrame];
        }
        
        public void LoadAnimationRandom(SOSpriteAnimation data)
        {
            _animation = data;
            LoadAnimation();
            _currentFrame = UnityEngine.Random.Range(0, _animation.Frames.Length);
        }
    }
}
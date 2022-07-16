using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Mechanics.Player;
using UnityEngine;

namespace Game.Mechanics.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatedSprite : MonoBehaviour
    {
        public float Length
        {
            get
            {
                return _animation.Frames.Length * _spf;
            }
        }
        
        [SerializeField]
        SOSpriteAnimation _animation;

        SpriteRenderer _renderer;
        float _spf;
        float _timer = 0;
        int _currentFrame;
        
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

            if (_timer > _spf)
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
            _currentFrame = (_currentFrame + 1) % _animation.Frames.Length;
            _renderer.sprite = _animation.Frames[_currentFrame];
        }

        public void LoadAnimation(SOSpriteAnimation data)
        {
            _animation = data;
            LoadAnimation();
        }
        
        void LoadAnimation()
        {
            _spf = 1f / _animation.Fps;
            _currentFrame = _animation.InitialFrame;
            _renderer.sprite = _animation.Frames[_currentFrame];
        }
    }
}
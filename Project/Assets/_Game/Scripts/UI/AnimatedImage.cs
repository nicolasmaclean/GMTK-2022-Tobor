using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Image))]
    public class AnimatedImage : MonoBehaviour
    {
        public float Spf { get; private set; }

        [SerializeField]
        SOSpriteAnimation _animation;

        Image _image;
        float _timer = 0;
        int _currentFrame;

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        void Start()
        {
            LoadAnimation();
        }

        void OnValidate()
        {
            if (!_image) _image = GetComponent<Image>();
            if (!_animation)
            {
                _image.sprite = null;
                return;
            }

            LoadAnimation();
        }

        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > Spf)
            {
                NextFrame();
                _timer = 0;
            }
        }

        void NextFrame()
        {
            _currentFrame++;
            if (_currentFrame >= _animation.Frames.Length)
            {
                _currentFrame = 0;
            }

            _image.sprite = _animation.Frames[_currentFrame];
        }

        void LoadAnimation()
        {
            Spf = 1f / _animation.Fps;
            _currentFrame = _animation.InitialFrame;
            _image.sprite = _animation.Frames[_currentFrame];
        }
    }
}
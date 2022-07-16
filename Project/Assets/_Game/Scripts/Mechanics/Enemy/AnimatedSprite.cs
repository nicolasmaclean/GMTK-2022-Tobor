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
        [SerializeField]
        Sprite[] _frames;

        [SerializeField]
        [Tooltip("Frames per second")]
        float _fps;

        [SerializeField]
        int _initialFrame = 0;

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
            _currentFrame = _initialFrame;
            _renderer.sprite = _frames[_currentFrame];
            UpdateSpf();
        }

        void OnValidate()
        {
            UpdateSpf();
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
            _currentFrame = (_currentFrame + 1) % _frames.Length;
            _renderer.sprite = _frames[_currentFrame];
        }

        void UpdateSpf()
        {
            _spf = 1f / _fps;
        }
    }
}
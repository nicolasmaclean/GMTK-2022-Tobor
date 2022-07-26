using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Utility.UI
{
    [RequireComponent(typeof(Image))]
    public class AnimatedImage : MonoBehaviour
    {
        public float Spf { get; private set; }

        [SerializeField]
        [ReadOnly]
        int _currentFrame;
        
        [FormerlySerializedAs("_animation"),SerializeField]
        public SOSpriteAnimation Animation;

        [FormerlySerializedAs("_oneShot"),SerializeField]
        public bool IsOneShot = false;

        Image _image;
        float _timer = 0;

        public static AnimatedImage Create(SOSpriteAnimation data)
        {
            AnimatedImage img = new GameObject(data.name).AddComponent<Image>().gameObject.AddComponent<AnimatedImage>();
            img.Animation = data;
            return img;
        }

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
            if (!Animation)
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
            if (_currentFrame >= Animation.Frames.Length)
            {
                if (IsOneShot)
                {
                    return;
                }
                _currentFrame = 0;
            }

            _image.sprite = Animation.Frames[_currentFrame];
        }

        void LoadAnimation()
        {
            Spf = 1f / Animation.Fps;
            _currentFrame = Animation.InitialFrame;
            _image.sprite = Animation.Frames[_currentFrame];
        }
    }
}
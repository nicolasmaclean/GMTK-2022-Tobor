using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Mechanics.Player;
using Game.Utility.UI;

namespace Game.UI.HUD
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        float _updateFactor = 15f;

        [SerializeField]
        Image _icon;

        [SerializeField]
        Sprite _heart;
        
        Slider _slider;
        float _target;

        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        void Start()
        {
            _slider.minValue = 0;
            _slider.maxValue = PlayerStats.Instance.ConstitutionRange.y;

            _slider.value = _target = PlayerController.Instance.Health;
        }

        Coroutine updateAnimation = null;
        public void UpdateHeatlh(float health)
        {
            if (health > _target)
            {
                _icon.sprite = _heart;
            }
            if (updateAnimation != null) StopCoroutine(updateAnimation);
            updateAnimation = StartCoroutine(Tween.SliderNonLerp(_slider, health, _updateFactor));
            _target = health;
        }
    }
}


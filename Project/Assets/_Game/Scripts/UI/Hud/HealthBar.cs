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
        
        Slider _slider;

        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        void Start()
        {
            _slider.minValue = 0;
            _slider.maxValue = PlayerStats.Instance.ConstitutionRange.y;

            _slider.value = PlayerController.Instance.Health;
        }

        Coroutine updateAnimation = null;
        public void UpdateHeatlh(float health)
        {
            if (updateAnimation != null) StopCoroutine(updateAnimation);
            updateAnimation = StartCoroutine(Tween.SliderNonLerp(_slider, health, _updateFactor));
        }
    }
}


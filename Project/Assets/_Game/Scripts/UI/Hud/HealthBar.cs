using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Mechanics.Player;

namespace Game.UI.HUD
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        Slider healthBarSlider;

        private void Start()
        {
            healthBarSlider = GetComponent<Slider>();

            healthBarSlider.minValue = 0;
            healthBarSlider.maxValue = PlayerStats.Instance.ConstitutionRange.y;
            Debug.Log(PlayerStats.Instance.ConstitutionRange.y.ToString());

            UpdateHeatlhBar();

        }

        public void UpdateHeatlhBar()
        {
            healthBarSlider.value = PlayerController.Instance.Health;
            Debug.Log(PlayerController.Instance.Health.ToString());
        }
    }
}


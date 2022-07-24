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
        private void Start()
        {
            Slider healthBarSlider = GetComponent<Slider>();

            healthBarSlider.maxValue = PlayerStats.Instance.ConstitutionRange.y;
            healthBarSlider.value = PlayerController.Instance.Health;
        }
    }
}


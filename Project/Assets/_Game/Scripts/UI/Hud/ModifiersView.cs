using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using TMPro;
using UnityEngine;

namespace Game.UI.Hud
{
    public class ModifiersView : MonoBehaviour
    {
        [SerializeField]
        TMP_Text[] _txt;

        void Start()
        {
            foreach (var txt in _txt)
            {
                txt.text = "X1";
            }
        }

        void OnEnable() => Modifiers.OnChange += UpdateText;

        void OnDisable() => Modifiers.OnChange -= UpdateText;

        void UpdateText()
        {
            const string prefix = "X";
            _txt[0].text = prefix + Modifiers.EnemyMultiplier;
            _txt[1].text = prefix + Modifiers.DamageMultiplier;
            _txt[2].text = prefix + Modifiers.SpeedMultiplier;
            _txt[3].text = prefix + Modifiers.AttackSpeedMultiplier;
            _txt[4].text = prefix + Modifiers.JumpMultiplier;
        }
    }
}
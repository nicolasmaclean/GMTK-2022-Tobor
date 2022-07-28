using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class ModifiersView : MonoBehaviour
    {
        [SerializeField]
        TMP_Text[] _txt;

        [SerializeField]
        Sprite[] _modifierSprites;

        [SerializeField]
        Image[] _modifierImages;

        // [SerializeField]
        // AnimationCurve _updateScale = AnimationCurve.Linear(0, 0, 1, 1);

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
            SetText(_txt[0], Modifiers.EnemyMultiplier, _modifierImages[0], _modifierSprites[0]);
            SetText(_txt[1], Modifiers.DamageMultiplier, _modifierImages[1], _modifierSprites[1]);
            SetText(_txt[2], Modifiers.SpeedMultiplier, _modifierImages[2], _modifierSprites[2]);
            SetText(_txt[3], Modifiers.AttackSpeedMultiplier, _modifierImages[3], _modifierSprites[3]);
            SetText(_txt[4], Modifiers.JumpMultiplier, _modifierImages[4], _modifierSprites[4]);

            void SetText(TMP_Text txt, float val, Image img, Sprite sprite)
            {
                const string prefix = "X";
                txt.text = prefix + val;
                if (Math.Abs(val - 1) > .01f)
                {
                    img.sprite = sprite;
                    // StartCoroutine(Tween.UseCurve(_updateScale, (val) =>
                    // {
                    //     txt.transform.localScale = Vector3.one * val;
                    // }));
                }
            }
        }
    }
}
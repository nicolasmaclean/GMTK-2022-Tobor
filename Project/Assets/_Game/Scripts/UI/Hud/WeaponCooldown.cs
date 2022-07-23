using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public class WeaponCooldown : MonoBehaviour
    {
        [SerializeField]
        float _fadeOutDuration;
        
        [Header("Sword")]
        [SerializeField]
        Image _swordCooldown;

        [SerializeField]
        Animator _swordAnimator;
        
        [Header("Bow")]
        [SerializeField]
        Image _bowCooldown;

        [SerializeField]
        Animator _bowAnimator;
        
        float _transitionLength;

        void Start()
        {
            HudController.OnShow += Show;
            HudController.OnHide += Hide;
        }

        void OnDestroy()
        {
            HudController.OnShow -= Show;
            HudController.OnHide -= Hide;
        }

        void Update()
        {
            UpdateSword(_swordCooldown, _swordAnimator);
            UpdateBow(_bowCooldown, _bowAnimator);
        }

        void Show()
        {
            gameObject.SetActive(true);
        }
        
        void Hide()
        {
            gameObject.SetActive(false);
        }

        void UpdateBow(Image img, Animator anim)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Reload"))
            {
                img.fillAmount = Mathf.Lerp(0, 1, state.normalizedTime);
                
                Color color = img.color;
                color.a = 1;
                img.color = color;
            }
            else if (state.IsName("Idle"))
            {
                float timeInIdle = state.normalizedTime * state.length;
                Color color = img.color;
                color.a = Mathf.Lerp(1, 0, timeInIdle / _fadeOutDuration);
                img.color = color;
            }
            else
            {
                Color color = img.color;
                color.a = 0;
                img.color = color;
            }
        }

        void UpdateSword(Image img, Animator anim)
        {
            AnimatorStateInfo state =      anim.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextState =  anim.GetNextAnimatorStateInfo(0);
            AnimatorTransitionInfo trans = anim.GetAnimatorTransitionInfo(0);
            
            if (anim.IsInTransition(0) && nextState.IsName("Idle"))
            {
                img.fillAmount = Mathf.Lerp(0, 1, trans.normalizedTime);
                
                Color color = img.color;
                color.a = 1;
                img.color = color;

                _transitionLength = trans.duration;
            }
            else if (state.IsName("Idle"))
            {
                float timeInIdle = state.normalizedTime * state.length - _transitionLength;
                Color color = img.color;
                color.a = Mathf.Lerp(1, 0, timeInIdle / _fadeOutDuration);
                img.color = color;
            }
            else
            {
                Color color = img.color;
                color.a = 0;
                img.color = color;
            }
        }
    }
}
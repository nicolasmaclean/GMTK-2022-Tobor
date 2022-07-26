using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.Mechanics.Level
{
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        Animator _anim;
        AnimateDissolve[] _brands;

        void Awake()
        {
            _brands = GetComponentsInChildren<AnimateDissolve>();
            _anim = GetComponent<Animator>();
        }

        public void Open()
        {
            _anim.SetBool(IsOpen, true);
            foreach (var brand in _brands)
            {
                brand.Out();
            }
        }
        
        static readonly int IsOpen = Animator.StringToHash("IsOpen");
    }
}
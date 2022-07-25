using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utility.UI;
using UnityEngine;

namespace Game.Utility
{
    [RequireComponent(typeof(Renderer))]
    public class AnimateDissolve : MonoBehaviour
    {
        [SerializeField]
        AnimationCurve _inCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [SerializeField]
        AnimationCurve _outCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public bool PlayOnStart = false;
        
        Material _mat;

        void Awake()
        {
            Renderer rend = GetComponent<Renderer>();
            _mat = rend.material;
        }

        void Start()
        {
            if (PlayOnStart)
            {
                In();
            }
        }

        public void In()
        {
            StartCoroutine(Tween.UseCurve(_inCurve, (val) =>
            {
                _mat.SetFloat(P_DISSOLVE, Mathf.Clamp(val, 0, 1));
            }));
        }

        public void Out()
        {
            StartCoroutine(Tween.UseCurve(_outCurve, (val) =>
            {
                _mat.SetFloat(P_DISSOLVE, val);
            }));
        }

        readonly int P_DISSOLVE = Shader.PropertyToID("_Dissolve");
    }
}
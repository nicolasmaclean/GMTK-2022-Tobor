using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Mechanics.Level
{
    public class ShootButton : MonoBehaviour
    {
        public UnityEvent OnShot;

        [SerializeField]
        [Utility.ReadOnly]
        bool _isEnabled;
    
        [SerializeField]
        bool _enabledOnStart = false;
    
        [SerializeField]
        [ColorUsage(false, true)]
        Color _enabled;
    
        [SerializeField]
        [ColorUsage(false, true)]
        Color _activated;
    
        Material _mat;

        void Awake()
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            _mat = rend.material;
        }

        void Start()
        {
            if (!_enabledOnStart) return;
            Enable();
        }
    
        public void Enable()
        {
            _mat.SetColor(S_EMISSION, _enabled);
            _isEnabled = true;
        }

        public void Activate()
        {
            if (!_isEnabled) return;
            _mat.SetColor(S_EMISSION, _activated);
            OnShot?.Invoke();
        }

        readonly static int S_EMISSION = Shader.PropertyToID("_EmissiveColor");
    }
}
using System.Collections;
using System.Collections.Generic;
using Game.Utility.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    [RequireComponent(typeof(Image))]
    public class ScreenFlash : MonoBehaviour
    {
        [SerializeField]
        Color _flashColor;

        [SerializeField]
        float _in;

        [SerializeField]
        float _out;

        Image _image;
        Color _defaultColor;

        Coroutine _currentCoroutine = null;
        
        void Awake()
        {
            _image = GetComponent<Image>();
            _defaultColor = _image.color;
        }

        public void Flash()
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(Tween.LerpColor(_image, _defaultColor, _flashColor, _in, () =>
            {
                _currentCoroutine = StartCoroutine(Tween.LerpColor(_image, _flashColor, _defaultColor, _out));
            }));
        }
    }
}
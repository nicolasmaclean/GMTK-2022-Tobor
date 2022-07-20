using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    [RequireComponent(typeof(Image))]
    public class Crosshair : MonoBehaviour
    {
        [SerializeField]
        Color _enemyColor = Color.red;
        
        Image _crosshair;
        RaycastHit hit;
        Color _defaultColor;

        void Awake()
        {
            _crosshair = GetComponent<Image>();
            _defaultColor = _crosshair.color;
        }
        
        void FixedUpdate() => EnemyHighlight();

        void EnemyHighlight()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            
            EnemyBase isEnemy = Physics.Raycast(ray, out hit) ? hit.transform.GetComponentInParent<EnemyBase>() : null;
            _crosshair.color = isEnemy ? _enemyColor : _defaultColor;
        }
    }
}
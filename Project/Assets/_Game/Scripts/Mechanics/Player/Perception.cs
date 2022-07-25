using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class Perception : MonoBehaviour
{
    [SerializeField]
    Vector2 _vignette = new Vector2(.1f, .55f);

    [SerializeField]
    Vector2 _aberration = new Vector2(.05f, .2f);
    
    Volume _volume;
    Vignette _vig;
    ChromaticAberration _ab;

    void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    void Start()
    {
        float t = PlayerStats.Instance.Perception / 20f;
        Debug.Log(t);
        if (_volume.profile.TryGet(out _vig))
        {
            _vig.intensity.value = Mathf.Lerp(_vignette.y, _vignette.x, t);
        }
        if (_volume.profile.TryGet(out _ab))
        {
            _ab.intensity.value = Mathf.Lerp(_aberration.y, _aberration.x, t);
        }
    }
}

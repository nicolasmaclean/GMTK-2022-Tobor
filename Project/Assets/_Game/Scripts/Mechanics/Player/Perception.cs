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
    Volume _volume;
    Vignette vig;

    void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    void Start()
    {
        if (_volume.profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value = PlayerStats.GetInRange(PlayerStats.Instance.Perception, PlayerStats.Instance.PerceptionRange);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

public class PlayerTrigger : MonoBehaviour
{
    public bool PlayerIsIn { get; private set; } = false;
    
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (!player) return;

        PlayerIsIn = true;
        OnEnter?.Invoke();
    }
    
    void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (!player) return;

        PlayerIsIn = false;
        OnExit?.Invoke();
    }
}

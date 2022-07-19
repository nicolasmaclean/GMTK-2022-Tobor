using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlayerTrigger : MonoBehaviour
{
    public bool PlayerIsIn { get; private set; } = false;

    void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (!parent) return;

        PlayerController player = parent.GetComponent<PlayerController>();
        if (!player) return;

        PlayerIsIn = true;
    }
    
    void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        if (!parent) return;

        PlayerController player = parent.GetComponent<PlayerController>();
        if (!player) return;

        PlayerIsIn = false;
    }
}

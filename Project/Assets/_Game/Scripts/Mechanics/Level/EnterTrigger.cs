using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnterTrigger : MonoBehaviour
{
    LevelController Controller;

    void Awake()
    {
        Controller = transform.parent.parent.GetComponent<LevelController>();
    }

    void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (!parent) return;

        PlayerController player = parent.GetComponent<PlayerController>();
        if (!player) return;

        Controller.MoveToNext();
    }
}

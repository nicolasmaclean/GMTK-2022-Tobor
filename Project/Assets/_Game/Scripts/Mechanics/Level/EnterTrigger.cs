using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Level;
using Game.Mechanics.Player;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Rigidbody))]
public class EnterTrigger : MonoBehaviour
{
    LevelController Controller;

    void Awake()
    {
        Controller = GetComponentInParent<LevelController>();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
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

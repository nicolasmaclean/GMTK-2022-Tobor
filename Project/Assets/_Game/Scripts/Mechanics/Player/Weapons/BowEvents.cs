using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using Game.Utility;
using UnityEngine;

public class BowEvents : MonoBehaviour
{
    AnimateDissolve _anim;

    void Awake()
    {
        _anim = GetComponentInChildren<AnimateDissolve>();
    }

    public void Fire()
    {
        PlayerController.Instance.FireArrow();
    }

    public void BrandIn()
    {
        _anim.In();
    }
}

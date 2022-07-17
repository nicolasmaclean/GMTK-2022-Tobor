using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMainCamera : MonoBehaviour
{
    Transform target;

    void Awake()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        var t = transform;
        t.position = target.position;
        t.rotation = target.rotation;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMainCamera : MonoBehaviour
{
    [SerializeField]
    bool x;
    
    [SerializeField]
    bool y;
    
    [SerializeField]
    bool z;

    Transform target;

    void Awake()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        Vector3 trot = target.transform.rotation.eulerAngles;

        if (x)
        {
            rot.x = trot.x;
        }
        if (y)
        {
            rot.y = trot.y;
        }
        if (z)
        {
            rot.z = trot.z;
        }

        transform.rotation = Quaternion.Euler(rot);
    }
}

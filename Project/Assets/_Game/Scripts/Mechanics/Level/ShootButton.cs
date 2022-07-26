using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ShootButton : MonoBehaviour
{
    [SerializeField]
    [ColorUsage(false, true)]
    Color _enabled;
    
    [SerializeField]
    [ColorUsage(false, true)]
    Color _activated;
    
    Material _mat;

    void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        _mat = rend.material;
    }
    
    public void Enable()
    {
        _mat.SetColor(S_EMISSION, _enabled);
    }

    public void Activate()
    {
        _mat.SetColor(S_EMISSION, _activated);
    }

    readonly static int S_EMISSION = Shader.PropertyToID("_EmissiveColor");
}

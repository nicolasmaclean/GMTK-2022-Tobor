using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Sprite Animation")]
public class SOSpriteAnimation : ScriptableObject
{
    public Sprite[] Frames;

    [Tooltip("Frames per second")]
    public float Fps;
    public int InitialFrame = 0;
}
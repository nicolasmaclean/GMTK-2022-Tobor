using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Data/Audio Clip")]
    public class SOAudioClip : ScriptableObject
    {
        public AudioClip Clip;

        [Range(0, 1)]
        public float Volume;
    }
}
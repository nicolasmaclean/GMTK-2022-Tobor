using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Data/Audio Clip")]
    public class SOAudioClip : ScriptableObject
    {
        public AudioClip Clip;

        [Range(0, 1)]
        public float Volume;
        
        public static void LoadSFX(AudioSource source, SOAudioClip clip)
        {
            source.clip = clip.Clip;
            source.volume = clip.Volume;
            source.Play();
        }
    }
}
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

        [Min(0)]
        public float StartEarly = 0f;

        [Range(-3, 3)]
        public float Pitch = 1;
        
        [Header("3D Settings")]
        [Range(0, 1)]
        public float SpatialBlend = 0f;

        [Range(0, 5)]
        public float DopplerLevel = 1;

        [Range(0, 360)]
        public float Spread = 0f;

        public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;

        [Range(0, 495)]
        public float MinDistance = 1f;

        [Min(1)]
        public float MaxDistance = 500;
        
        
        
        public static void LoadSFX(AudioSource source, SOAudioClip clip)
        {
            source.clip = clip.Clip;
            source.volume = clip.Volume;
            source.pitch = clip.Pitch;
            
            source.spatialBlend = clip.SpatialBlend;
            source.dopplerLevel = clip.DopplerLevel;
            source.spread = clip.Spread;
            source.rolloffMode = clip.RolloffMode;
            source.minDistance = clip.MinDistance;
            source.maxDistance = clip.MaxDistance;

            source.time = clip.StartEarly;
            
            source.Play();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.Core
{
    public class MusicManager : LazySingleton<MusicManager>
    {
        AudioSource _source;
        
        public void Play(SOAudioClip clip)
        {
            if (!clip) return;
            if (!clip.Clip && clip.Clips != null && clip.Clips.Length == 0) return;

            if (!_source) _source = gameObject.AddComponent<AudioSource>();
            SOAudioClip.LoadSFX(_source, clip);
        }
    }
}
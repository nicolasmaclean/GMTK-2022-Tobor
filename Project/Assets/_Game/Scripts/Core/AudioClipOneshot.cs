using Game.Utility;
using TMPro;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipOneshot : MonoBehaviour
    {
        public SOAudioClip Clip;

        AudioSource _source;
        
        public static AudioClipOneshot Create()
        {
            return new GameObject("SFX").AddComponent<AudioClipOneshot>();
        }

        public static AudioClipOneshot Create(SOAudioClip clip)
        {
            AudioClipOneshot shot = Create();
            shot.Clip = clip;
            return shot;
        }

        void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        void Start() => PlayClip();

        void PlayClip()
        {
            _source.clip = Clip.Clip;
            _source.volume = Clip.Volume;
            _source.Play();
            
            Destroy(this, Clip.Clip.length);
        }
    }
}
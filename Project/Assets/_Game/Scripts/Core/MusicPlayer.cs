using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class MusicPlayer : MonoBehaviour
    {
        public AudioClip gameMusic;
        private AudioSource track1, track2;
        private bool isPlayingTrack;

        [Range(0, 1)]
        public float Volume;

        //Lazy instantiation
        private static MusicPlayer _instance;
        public static MusicPlayer instance
        {
            get
            {
                if(_instance == null)
                {
                    //Check if it exists
                    _instance = FindObjectOfType<MusicPlayer>();
                    if(_instance == null)
                    {
                        //Create MusicPlayer
                        GameObject single = new GameObject("MusicPlayer");
                        _instance = single.AddComponent<MusicPlayer>();

                        DontDestroyOnLoad(single);
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
        }

        private void Start()
        {
            track1 = gameObject.AddComponent<AudioSource>();
            track2 = gameObject.AddComponent<AudioSource>();

            isPlayingTrack = true;
        }

        public void SwapTrack(AudioClip newClip)
        {
            StopAllCoroutines();
            
            StartCoroutine(FadeTrack(newClip));

            isPlayingTrack = !isPlayingTrack;
        }

        public void ReturnDefault()
        {
            SwapTrack(gameMusic);
        }

        private IEnumerator FadeTrack(AudioClip newClip)
        {
            float timeToFade = 1.50f;
            float timeElapsed = 0;

            if(isPlayingTrack)
            {
                track2.clip = newClip;
                track2.Play();

                while(timeElapsed < timeToFade)
                {
                    track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                    track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                track1.Stop();
            }
            else
            {
                track1.clip = newClip;
                track1.Play();

                while(timeElapsed < timeToFade)
                {
                    track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                    track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                track2.Stop();
            }
        }



    }
}

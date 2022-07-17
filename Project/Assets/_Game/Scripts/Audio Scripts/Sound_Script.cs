using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound_Script : MonoBehaviour
{
    [SerializeField] AudioMixer gameMixer;
    [SerializeField] AudioSource sfxSources;
    [SerializeField] List<AudioClip> sfxClips = new List<AudioClip>();

    public static Sound_Script instances;

    AudioSource bgmSources;
    AudioClip menuBGM;
    AudioClip levelBGM1;
    AudioClip levelBGM2;
    private void Awake() {
        if(instances == null)
        {
            instances = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void stepSound()
    {
        AudioClip sfxClip = sfxClips[Random.Range(0, sfxClips.Count)];
        sfxSources.PlayOneShot(sfxClip);
    }

    public void menuMusic()
    {

    }

    public void bgmLevel()
    {

    }

    public void swordSlash()
    {

    }
}
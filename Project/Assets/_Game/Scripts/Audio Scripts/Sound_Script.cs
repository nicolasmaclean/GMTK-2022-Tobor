using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound_Script : MonoBehaviour
{
    [SerializeField] AudioMixer gameMixer;

    public static Sound_Script instances;

    AudioSource sfxSources;
    AudioClip steps;
    AudioClip slash;

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
        sfxSources.PlayOneShot(stepSound);
    }

    public void menuMusic()
    {
        bgmSources.PlayOneShot(menuBGM);
    }

    public void bgmLevel()
    {
        bgmSources.PlayOneShot(bgmLevel);
    }

    public void swordSlash()
    {
        sfxSources.PlayOneShot(slash);
    }
}
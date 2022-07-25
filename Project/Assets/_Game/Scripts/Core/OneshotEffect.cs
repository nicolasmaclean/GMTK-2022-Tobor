using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game.Core
{
    public class OneshotEffect : MonoBehaviour
    {
        static Transform _organizer = null;
        public static Transform Organizer
        {
            get
            {
                if (_organizer == null)
                {
                    _organizer = new GameObject("=== One Shot Effects ===").transform;
                }

                return _organizer;
            }
        }

        [SerializeField]
        SOAudioClip _clip;
        
        [SerializeField]
        [Utility.ReadOnly]
        ParticleSystem _vfx;

        public void Play(Vector3 position) => Play(position, Vector3.up);
        public void Play(Vector3 position, Vector3 normal)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.zero, normal);
            GameObject vfx = GetComponent<ParticleSystem>()?.gameObject;
            if (vfx != null)
            {
                PlayVFX(vfx.gameObject, position, rot);
            }
            else
            {
                PlaySFX(_clip, position, rot);
            }
        }
        
        public static GameObject PlaySFX(SOAudioClip clip, Vector3 position, Quaternion rotation)
        {
            GameObject go = PlaySFX(clip);
            
            go.transform.position = position;
            go.transform.rotation = rotation;

            return go;
        }
        
        public static GameObject PlaySFX(SOAudioClip clip)
        {
            GameObject go = new GameObject(clip.name);
            go.transform.parent = Organizer;
            OneshotEffect effect = go.AddComponent<OneshotEffect>();
            effect.Play(clip);
            
            return go;
        }

        public static GameObject PlayVFX(GameObject vfx, Vector3 position, Quaternion rotation)
        {
            GameObject go = Instantiate(vfx, position, rotation, Organizer);
            
            OneshotEffect effect = go.GetComponent<OneshotEffect>() ?? go.AddComponent<OneshotEffect>();
            effect.Play(go.GetComponent<ParticleSystem>());
            
            return go;
        }
        
        void Play(SOAudioClip clip)
        {
            _clip = clip;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            SOAudioClip.LoadSFX(source, clip);
            Destroy(gameObject, clip.Clip.length);
        }

        void Play(ParticleSystem vfx)
        {
            _vfx = vfx;
            
            vfx.Play();
            Destroy(gameObject, vfx.main.duration);
        }
    }
}
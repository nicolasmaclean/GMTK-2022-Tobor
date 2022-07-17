using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Core
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Create();
                }

                return _instance;
            }
        }
        static SFXManager _instance = null;

        static void Create()
        {
            _instance = new GameObject("SFXManager").AddComponent<SFXManager>();
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public static void PlaySFX(SOAudioClip clip)
        {
            if (clip == null || clip.Clip == null) return;
            AudioClipOneshot.Create(clip);
        }
    }
}
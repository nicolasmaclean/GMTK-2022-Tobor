using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class MusicSwap : ScriptableObject
    {
        public AudioClip newtrack;

        [Range(0, 1)]
        public float Volume;

        //Detect if Player crosses collider to change tracks

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                MusicPlayer.instance.SwapTrack(newtrack);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                MusicPlayer.instance.SwapTrack(newtrack);
            }
        }
    }
}
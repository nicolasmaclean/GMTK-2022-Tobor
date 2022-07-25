using UnityEngine;

namespace Game.Core
{
    public class MonoExtended : MonoBehaviour
    {
        public void PlaySFX(SOAudioClip clip)
        {
            if (clip == null || (clip.Clip == null && clip.Clips.Length == 0)) return;
            OneshotEffect.PlaySFX(clip);
        }
    }
}
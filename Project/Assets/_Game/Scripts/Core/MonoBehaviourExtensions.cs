using UnityEngine;

namespace Game.Core
{
    public class MonoExtended : MonoBehaviour
    {
        public void PlaySFX(SOAudioClip clip)
        {
            if (clip == null || clip.Clip == null) return;
            OneshotEffect.PlaySFX(clip);
        }
    }
}
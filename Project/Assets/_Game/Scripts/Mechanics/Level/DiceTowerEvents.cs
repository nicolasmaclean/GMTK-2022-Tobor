using UnityEngine;

namespace Game.Mechanics.Level
{
    public class DiceTowerEvents : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem _smoke1;
        
        [SerializeField]
        ParticleSystem _smoke2;
        void Smoke1()
        {
            _smoke1.Play();
        }

        void Smoke2()
        {
            _smoke2.Play();
        }
    }
}
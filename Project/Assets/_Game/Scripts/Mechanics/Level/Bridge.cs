using UnityEngine;

namespace Game.Mechanics.Level
{
    [RequireComponent(typeof(Animator))]
    public class Bridge : MonoBehaviour
    {
        static readonly int AT_BRIDGE_OPEN = Animator.StringToHash("Risen");

        Animator _anim;
        
        void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void Rise()
        {
            _anim.SetBool(AT_BRIDGE_OPEN, true);
        }
    }
}
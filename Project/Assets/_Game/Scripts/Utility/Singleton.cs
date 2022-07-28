using UnityEngine;

namespace Game.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => _instance;
        static T _instance;

        void Awake()
        {
            // enforce single instance rule
            if (_instance == null)
            {
                _instance = this as T;
                OnAwake();
            }
            else
            {
                Destroy(this);
            }
        }
        
        protected virtual void OnAwake() { }

        void OnDestroy()
        {
            if (_instance != this) return;

            _instance = null;
            OnDestroyed();
        }
        protected virtual void OnDestroyed() { }
    }
}
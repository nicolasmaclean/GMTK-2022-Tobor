using UnityEngine;

namespace Game.Utility
{
    public class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    Create();
                }

                return _instance;
            }
        }

        static void Create()
        {
            DontDestroyOnLoad(new GameObject(typeof(T).ToString()).AddComponent<T>().gameObject);
        }

        void Awake()
        {
            if (_instance)
            {
                Destroy(this);
                return;
            }
            
            _instance = this as T;
            OnAwake();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnDestroy()
        {
            if (_instance != this) return;

            _instance = null;
            Destroyed();
        }
        
        protected virtual void Destroyed() { }
    }
}
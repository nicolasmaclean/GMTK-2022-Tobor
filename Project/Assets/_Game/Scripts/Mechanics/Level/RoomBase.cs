using System;
using UnityEngine;

namespace Game.Mechanics.Level
{
    public class RoomBase : MonoBehaviour
    {
        public LevelController Controller { get; private set; }
        
        [SerializeField]
        protected GameObject _doorExit;

        void Awake()
        {
            this.enabled = false;
            // OnAwake();
        }

        protected virtual void OnAwake()
        {
            Controller = GetComponentInParent<LevelController>();
            if (Controller == null) Debug.LogError("Unable to find parent Level Controller.");
        }

        public virtual void Load()
        {
            gameObject.SetActive(true);
            _doorExit.SetActive(true);
        }

        public virtual void Done()
        {
            OpenDoor();
        }

        public virtual void Close()
        {
            CloseDoor();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        void OpenDoor()
        {
            if (!_doorExit) return;
            _doorExit.SetActive(false);
        }

        void CloseDoor()
        {
            if (!_doorExit) return;
            _doorExit.SetActive(true);
        }
    }
}
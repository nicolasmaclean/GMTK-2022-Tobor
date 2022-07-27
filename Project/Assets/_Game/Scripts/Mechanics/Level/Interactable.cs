using System;
using Game.Mechanics.Player;
using Game.UI.Hud;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Mechanics.Level
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent OnInteract;

        [SerializeField]
        InteractionType _type = InteractionType.None;
        
        PlayerTrigger _playerTrigger;
        KeyCode _key;

        void Awake()
        {
            _playerTrigger = GetComponentInChildren<PlayerTrigger>();
        }

        void Start()
        {
            _key = PlayerController.Instance.InteractKey;
        }
        
        void OnEnable()
        {
            _playerTrigger.OnEnter.AddListener(Prompt);
            _playerTrigger.OnExit.AddListener(Hide);
        }
        
        void OnDisable()
        {
            _playerTrigger.OnEnter.RemoveListener(Prompt);
            _playerTrigger.OnExit.RemoveListener(Hide);
        }

        void Update()
        {
            if (!_playerInRange) return;
            if (!Input.GetKeyDown(_key)) return;

            Hide();
            OnInteract?.Invoke();
            this.enabled = false;

            if (_type == InteractionType.Brand)
            {
                PlayerController.Instance.PlayBrandAnimation();
            }
            else if (_type == InteractionType.Weapons)
            {
                PlayerController.Instance.PlayPickupAnimation();
            }
        }

        bool _playerInRange = false;
        void Prompt()
        {
            ClickPrompt.Instance.Show();
            _playerInRange = true;
        }

        void Hide()
        {
            ClickPrompt.Instance.Hide();
            _playerInRange = false;
        }

        enum InteractionType
        {
            None = 0, Brand = 1, Weapons = 2
        }
    }
}
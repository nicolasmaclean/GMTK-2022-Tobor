using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Mechanics.Player;
using Game.UI.Hud;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Game.Mechanics.Level
{
    public class DiceTower : MonoBehaviour
    {
        static KeyCode _interactKey = KeyCode.None;
        static KeyCode S_interactKey
        {
            get
            {
                if (_interactKey == KeyCode.None)
                {
                    _interactKey = PlayerController.Instance.InteractKey;
                }

                return _interactKey;
            }
        }
        
        [SerializeField]
        [Utility.ReadOnly]
        bool _haveRolled = false;

        [Header("Visuals")]
        [SerializeField]
        GameObject[] _dice;

        [FormerlySerializedAs("_diceValues"),SerializeField]
        Texture[] _diceTextures;

        [SerializeField]
        Texture[] _displayTextures;

        [SerializeField]
        Renderer _displayRenderer;

        [Header("Colliders")]
        [SerializeField]
        PlayerTrigger _approachTrigger;

        [SerializeField]
        PlayerTrigger _rollTrigger;

        Animator _animator;
        Material _displayMaterial;
        bool _approached;
        int _diceAmount;

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _displayMaterial = _displayRenderer.material;
            _displayMaterial.mainTexture = _displayTextures[0];
        }

        void Start()
        {
            _rollTrigger.OnEnter.AddListener(ShowPrompt);
            _rollTrigger.OnExit.AddListener(HidePrompt);
        }

        void OnEnable()
        {
            _approachTrigger.OnEnter.AddListener(Approach);
        }

        void Approach()
        {
            _approachTrigger.OnEnter.RemoveListener(Approach);
            _animator.SetBool(AT_APPROACH, true);
            _approached = true;
        }

        Coroutine _approachWaitCoroutine;
        void ShowPrompt()
        {
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("OnApproach"))
            {
                if (_approachWaitCoroutine != null) StopCoroutine(_approachWaitCoroutine);
                _approachWaitCoroutine = StartCoroutine(WaitTillApproached());
            }
            else
            {
                Show();
            }

            IEnumerator WaitTillApproached()
            {
                yield return null;
                yield return new WaitUntil(() =>
                {
                    AnimatorStateInfo s = _animator.GetCurrentAnimatorStateInfo(0);
                    return s.IsName("IdleAwake") && !_animator.IsInTransition(0);
                });

                if (_rollTrigger.PlayerIsIn)
                {
                    Show();
                }
            }

            void Show()
            {
                InteractPrompt.Instance.Show();
                InteractPrompt.Instance.OnReset -= UpdateDisplay;
                InteractPrompt.Instance.OnReset += UpdateDisplay;
            }
        }

        void HidePrompt()
        {
            InteractPrompt.Instance.Hide();
            InteractPrompt.Instance.OnReset -= UpdateDisplay;
        }

        void Update()
        {
            // player is in range
            if (!_rollTrigger.PlayerIsIn) return;
            
            // dice have not been rolled and 
            if (_haveRolled || !_approached) return;

            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("IdleAwake") || _animator.IsInTransition(0)) return;

            if (Input.GetKeyDown(S_interactKey))
            {
                InteractPrompt.Instance.Pressed = true;
            }
            else if (Input.GetKeyUp(S_interactKey) && InteractPrompt.Instance.Pressed)
            {
                if (_diceAmount != 0)
                {
                    Roll(_diceAmount);
                    _haveRolled = true;
                    
                    InteractPrompt.Instance.Hide();
                    _rollTrigger.OnEnter.RemoveListener(ShowPrompt);
                    _rollTrigger.OnExit.RemoveListener(HidePrompt);
                }
                else
                {
                    InteractPrompt.Instance.Reset();

                    _diceAmount = 0;
                    _displayMaterial.mainTexture = _displayTextures[_diceAmount];
                }
            }
        }

        void UpdateDisplay()
        {
            _diceAmount = (_diceAmount + 1) % 6;
            if (_diceAmount == 0) _diceAmount++;
            
            _displayMaterial.mainTexture = _displayTextures[_diceAmount];
        }

        void Roll(int numberOfDice)
        {
            int[] rolls = new int[6] { 0, 0, 0, 0, 0, 0 };
            
            for (int i = 0; i < _dice.Length; i++)
            {
                GameObject go = _dice[i];
                if (i < numberOfDice)
                {
                    int roll = UnityEngine.Random.Range(1, 6);
                    rolls[roll-1]++;
                    
                    go.SetActive(true);
                    ShowDie(go, roll);
                }
                else
                {
                    go.SetActive(false);
                }
            }
            
            _animator.SetBool(AT_ROLL, true);
            Modifiers.SetMultipliers(rolls[0], rolls[1], rolls[2], rolls[3], rolls[4], rolls[5]);
            
            #if UNITY_EDITOR
            int index = SceneManager.GetActiveScene().buildIndex;
            if (index == -1 || 3 <= index) return;
            #endif
            StartCoroutine(WaitThen(3.3f, () =>
            {
                LevelController.CurrentRoom.Done();
            }));
        }

        void ShowDie(GameObject die, int num)
        {
            Renderer rend = die.GetComponent<Renderer>();
            Material mat = rend.material;
            mat.mainTexture = _diceTextures[num-1];
        }

        IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }

        readonly String AT_ROLL = "Rolled";
        readonly String AT_APPROACH = "Approached";
    }
}
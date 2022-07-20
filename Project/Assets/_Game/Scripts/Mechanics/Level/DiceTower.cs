using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Game.Mechanics.Level
{
    public class DiceTower : MonoBehaviour
    {
        [SerializeField]
        [Utility.ReadOnly]
        bool _haveRolled = false;
        
        [SerializeField]
        GameObject[] _dice;

        [SerializeField]
        Texture[] _diceValues;
        
        DiceTowerTrigger _trigger;
        Animator _animator;

        void Awake()
        {
            _trigger = GetComponentInChildren<DiceTowerTrigger>();
            _animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (_haveRolled) return;
            
            if (Input.GetKeyDown(KeyCode.E) && _trigger.PlayerInside)
            {
                Roll(3);
                _haveRolled = true;
            }
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
            
            _animator.SetTrigger(AT_Roll);
            Modifiers.SetMultipliers(rolls[0], rolls[1], rolls[2], rolls[3], rolls[4], rolls[5]);
            StartCoroutine(WaitThen(3.3f, () =>
            {
                LevelController.CurrentRoom.Done();
            }));
        }

        void ShowDie(GameObject die, int num)
        {
            Renderer rend = die.GetComponent<Renderer>();
            Material mat = rend.material;
            mat.mainTexture = _diceValues[num-1];
        }

        IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }

        readonly String AT_Roll = "Roll";
    }
}
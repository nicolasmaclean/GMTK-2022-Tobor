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
            if (Input.GetKeyDown(KeyCode.E) && _trigger.PlayerInside)
            {
                Roll(3);
            }
        }

        void Roll(int numberOfDice)
        {
            List<int> rolls = new List<int>();
            
            for (int i = 0; i < _dice.Length; i++)
            {
                GameObject go = _dice[i];
                if (i < numberOfDice)
                {
                    int roll = UnityEngine.Random.Range(1, 6);
                    rolls.Add(roll);
                    
                    go.SetActive(true);
                    ShowDie(go, roll);
                }
                else
                {
                    go.SetActive(false);
                }
            }
            
            _animator.SetTrigger(AT_Roll);
            Debug.Log(String.Join(" ", rolls));
        }

        void ShowDie(GameObject die, int num)
        {
            Renderer rend = die.GetComponent<Renderer>();
            Material mat = rend.material;
            mat.SetTexture(BaseColor, _diceValues[num]);
        }

        readonly String AT_Roll = "Roll";
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    }
}
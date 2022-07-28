using System;
using Game.Mechanics.Enemy;
using Game.Mechanics.Player;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Mechanics.Level
{
    public class EnemiesKilled : MonoBehaviour
    {
        public UnityEvent OnEnemiesKilled;

        [SerializeField]
        [Utility.ReadOnly]
        int _enemiesLeft;

        [SerializeField]
        GameObject _default;
        
        [SerializeField]
        GameObject _x2;
        
        [SerializeField]
        GameObject _x4;
        
        [SerializeField]
        GameObject _x6;
        
        [SerializeField]
        GameObject _x8;

        void Awake()
        {
            // disable extra enemies by default
            foreach (var em in new GameObject[] { _x2, _x4, _x6, _x8 })
            {
                em.SetActive(false);
            }
            
            // count default enemies
            foreach (EnemyBase em in _default.GetComponentsInChildren<EnemyBase>())
            {
                em.OnKilled += EnemyKilled;
                _enemiesLeft++;
            }
            
            Modifiers.OnChange += ApplyModifiers;
        }

        void OnDestroy()
        {
            Modifiers.OnChange -= ApplyModifiers;
        }

        void ApplyModifiers()
        {
            for (int i = 2; i <= Modifiers.EnemyMultiplier; i++)
            {
                switch (i)
                {
                    case 2:
                        Activate(_x2);
                        break;

                    case 4:
                        Activate(_x4);
                        break;

                    case 6:
                        Activate(_x6);
                        break;

                    case 8:
                        Activate(_x8);
                        break;
                }
            }

            void Activate(GameObject go)
            {
                if (go.activeSelf) return;
                
                go.SetActive(true);
                foreach (EnemyBase em in go.GetComponentsInChildren<EnemyBase>())
                {
                    em.OnKilled += EnemyKilled;
                    _enemiesLeft++;
                }
            }
        }

        void EnemyKilled(EnemyBase em)
        {
            em.OnKilled -= EnemyKilled;
            _enemiesLeft--;

            if (_enemiesLeft <= 0)
            {
                OnEnemiesKilled?.Invoke();
            }
        }

        public void Win()
        {
            GameMenuController.Win();
        }
    }
}
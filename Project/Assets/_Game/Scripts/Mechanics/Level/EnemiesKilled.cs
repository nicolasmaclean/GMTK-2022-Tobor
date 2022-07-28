using Game.Mechanics.Enemy;
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

        void Awake()
        {
            foreach (EnemyBase em in GetComponentsInChildren<EnemyBase>())
            {
                em.OnKilled += EnemyKilled;
                _enemiesLeft++;
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
    }
}
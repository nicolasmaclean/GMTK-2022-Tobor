using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Mechanics.Enemy;

namespace Game.Mechanics.Level
{
    public class Room : RoomBase
    {
        public int EnemiesLeft { get; private set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            InitialCountEnemies();
        }

        void InitialCountEnemies()
        {
            EnemyBase[] enemies = GetComponentsInChildren<EnemyBase>(includeInactive: true);
            foreach (EnemyBase em in enemies)
            {
                em.OnKilled += UpdateEnemyCount;
            }
            
            EnemiesLeft = enemies.Length;
        }

        void UpdateEnemyCount(EnemyBase em)
        {
            EnemiesLeft--;
            em.OnKilled -= UpdateEnemyCount;

            if (EnemiesLeft <= 0)
            {
                Done();
            }
        }
    }
}
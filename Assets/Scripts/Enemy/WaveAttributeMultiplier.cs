using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    /// <summary>
    /// Handles calculating and scaling enemy stats
    /// </summary>
    [Serializable]
    public class WaveAttributeMultiplier
    {
        [Header("Enemy stat settings")]
        [Tooltip("Whether or not to increase enemy stats per wave")] [SerializeField]
        private bool increaseStats;

        [Tooltip("Incrementing value used in attribute calculations")] [SerializeField]
        private int statIncrementer;

        [Tooltip("A random amount enemy stats are multiplied by")]
        private int _deviatedMultiplier;
        
        [Range(1, 10)] [SerializeField]
        private int randomDeviation;
        
        [Tooltip("How much to increment statIncrementer by. Defaults to 1")] [SerializeField]
        private  int statIncrement = 1;

        public void MultiplyEnemyStats(GameObject enemy)
        {
            if (!increaseStats) return;
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            AnimatedCollisionDamager enemyDamage =
                enemy.GetComponentInChildren<AnimatedCollisionDamager>();
            
            // Randomize multiplier
            _deviatedMultiplier = statIncrementer + Random.Range(-randomDeviation, randomDeviation);
            if (_deviatedMultiplier < 0)
            {
                _deviatedMultiplier = 1 / (_deviatedMultiplier * -1);
            }

            Debug.Log("Random multiplier is " + _deviatedMultiplier);
            // Set random stats
            enemyHealth.ScaleHealth(_deviatedMultiplier);
            Debug.Log(enemy.name + " Health: " + enemyHealth.GetHealth());

            enemyDamage.damage += _deviatedMultiplier * enemyDamage.damage;
            Debug.Log(enemy.name + " Damage: " + enemyDamage.damage);

            _deviatedMultiplier = 0;
        }
        
        public void Increment()
        {
            statIncrementer += statIncrement;
            randomDeviation += statIncrement;
        }
    }
}

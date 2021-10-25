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
        private int multiplierValue;

        [Range(0.1f, 1f)] [SerializeField]
        private float randomDeviationMax;
        
        [Tooltip("How much to increment statIncrementer by. Defaults to 1")] [SerializeField]
        private  int multiplierIncrement = 1;

        public void MultiplyEnemyStats(GameObject enemy)
        {
            if (!increaseStats) return;
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            AnimatedCollisionDamager enemyDamage =
                enemy.GetComponentInChildren<AnimatedCollisionDamager>();

            // Randomize multiplier
            float deviatedMultiplier = multiplierValue + Random.Range(-randomDeviationMax, randomDeviationMax);
            
            // Set stats
            enemyHealth.ScaleHealth(deviatedMultiplier);
            enemyDamage.ScaleDamage(deviatedMultiplier);
        }
        
        public void Increment()
        {
            multiplierValue += multiplierIncrement;
        }
    }
}

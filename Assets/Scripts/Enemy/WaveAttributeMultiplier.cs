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

        [Tooltip("The base value of _statIncrementer")]
        public int incrementerBase = 1;

        [Tooltip("Incrementing value used in attribute calculations")] [HideInInspector]
        public int statIncrementer;

        [Tooltip("A random amount enemy stats are multiplied by")]
        private int _deviatedMultiplier;

        [Range(1, 5)]
        public int randomDeviationMin;
        [Range(1, 10)]
        public int randomDeviationMax;
        
        [Tooltip("How much to increment statIncrementer by. Defaults to 1")]
        public int statIncrement = 1;

        public void CalculateEnemyStats(GameObject enemy)
        {
            if (!increaseStats) return;
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            AnimatedCollisionDamager enemyDamage =
                enemy.GetComponentInChildren<AnimatedCollisionDamager>();
                
            // Determines whether deviation will be positive or negative
            int deviationDecider = Random.Range(1, 3); 
                    
            if (deviationDecider == 1)
            {
                int negativeDeviation = Random.Range(statIncrementer, randomDeviationMin + 1); 
                _deviatedMultiplier = 1 / negativeDeviation;
            }

            if (deviationDecider == 2)
            {
                int positiveDeviation = Random.Range(statIncrementer, randomDeviationMax + 1);
                _deviatedMultiplier = positiveDeviation;
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
            randomDeviationMin += statIncrement;
            randomDeviationMax += statIncrement;
        }
    
        // Cant put this in start because not a MonoBehaviour, doing so would not allow me
        // to show in the inspector
        public void SetIncrementer()
        {
            statIncrementer = incrementerBase;
        }
    }
}

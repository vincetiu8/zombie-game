using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    /// <summary>
    /// Handles calculating and scaling enemy stats
    /// </summary>
    public class WaveAttributeMultiplier : MonoBehaviour
    {
        [Header("Enemy stat settings")]
        [Tooltip("Whether or not to increase enemy stats per wave")]
        public bool increaseStats;

        [Tooltip("Whether or not to randomly deviate from the attribute multiplier")]
        public bool fixedMultiplier;

        [Tooltip("Whether or not to reset the stat increase after all waves are completed")]
        public bool resetStatIncrease;
		
        [Tooltip("The base value of _statIncrementer")]
        public int incrementerBase = 1;

        [Tooltip("Incrementing value used in attribute calculations")] [HideInInspector]
        public int statIncrementer;

        [Tooltip("How much to divide health/damage cacluclations by to get new stats")]
        public int fixedStatDivider;

        [Tooltip("A random amount enemy stats are multiplied by")]
        private int _deviatedFixedMultiplier;

        [Range(1, 5)]
        public int randomDeviationMin;
        [Range(1, 10)]
        public int randomDeviationMax;

        [Header("Increment settings")]
        [Tooltip("How much to increment _fixedStatIncrementer by. Defaults to 1")]
        public int fixedStatIncrement = 1;

        public void CalculateEnemyStats(GameObject enemy)
        {
            if (increaseStats)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                AnimatedCollisionDamager enemyDamage =
                    enemy.GetComponentInChildren<AnimatedCollisionDamager>();

                if (!fixedMultiplier)
                {
                    // Determines whether deviation will be positive or negative
                    int deviationDecider = Random.Range(1, 3); 
                    
                    do
                    {
                        if (deviationDecider == 1)
                        {
                            int negativeDeviation = Random.Range(statIncrementer, randomDeviationMin + 1); 
                            _deviatedFixedMultiplier = 1 / negativeDeviation;
                            break;
                        }

                        if (deviationDecider == 2)
                        {
                            int positiveDeviation = Random.Range(statIncrementer, randomDeviationMax + 1);
                            _deviatedFixedMultiplier = positiveDeviation;
                            break;
                        }
                    } while (false);

                    Debug.Log("Random multiplier is " + _deviatedFixedMultiplier);
                    // Set random stats
                    enemyHealth.ScaleHealth(_deviatedFixedMultiplier);
                    Debug.Log(enemy.name + " Health: " + enemyHealth.GetHealth());

                    enemyDamage.damage += _deviatedFixedMultiplier * enemyDamage.damage;
                    Debug.Log(enemy.name + " Damage: " + enemyDamage.damage);

                    _deviatedFixedMultiplier = 0;
                    return;
                }

                // Set fixed stats
                enemyHealth.ChangeHealth(statIncrementer * enemyHealth.GetHealth() / fixedStatDivider);
                Debug.Log(enemy.name + " Health: " + enemyHealth.GetHealth());

                enemyDamage.damage = statIncrementer * enemyDamage.damage / fixedStatDivider;
                Debug.Log(enemy.name + " Damage: " + enemyDamage.damage);
            }
        }
    }
}

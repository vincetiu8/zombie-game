using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class WaveAttributeMultiplier : MonoBehaviour
    {
        [Header("Enemy stat settings")]
        [Tooltip("Whether or not to increase enemy stats per wave")]
        public bool increaseStats;

        [Tooltip("Whether or not to use a fixed stat multiplier")]
        public bool fixedMultiplier;

        [Tooltip("Whether or not to reset the stat increase after all waves are completed")]
        public bool resetStatIncrease;
		
        [Tooltip("A fixed amount enemy stats are multiplied by. Increments per wave, defaults to 1")]
        public int fixedStatMultiplier = 1;

        [Tooltip("The incrementing value of _fixedStatMultiplier")] [HideInInspector]
        public int _fixedStatIncrementer;

        [Tooltip("How much to divide health/damage cacluclations by to get new stats")]
        public int fixedStatDivider;

        [Tooltip("A random amount enemy stats are multiplied by")]
        private int _randomStatMultiplier;

        [Range(50, 99)] 
        public int randomStatMin;
        [Range(100, 300)] 
        public int randomStatMax;

        [Header("Increment settings")]
        [Tooltip("How much to increment _fixedStatIncrementer by. Defaults to 1")]
        public int fixedStatIncrement = 1;
        
        [Tooltip("How much to increment _randomStatMin by. Defaults to 5")]
        public int randomMinIncrement = 5;

        [Tooltip("How much to increment _randomStatMax by. Defaults to 5")] 
        public int randomMaxIncrement = 5;

        public void CalculateEnemyStats(GameObject enemy)
        {
            if (increaseStats)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                AnimatedCollisionDamager enemyDamage =
                    enemy.GetComponentInChildren<AnimatedCollisionDamager>();

                if (!fixedMultiplier)
                {
                    // Convert to percentage
                    _randomStatMultiplier = Random.Range(randomStatMin, randomStatMax) / 100;

                    // Set random stats
                    enemyHealth.ScaleHealth(_randomStatMultiplier);
                    Debug.Log(enemy.name + " Health: " + enemyHealth.GetHealth());

                    enemyDamage.damage += _randomStatMultiplier * enemyDamage.damage;
                    Debug.Log(enemy.name + " Damage: " + enemyDamage.damage);

                    return;
                }

                // Set fixed stats
                enemyHealth.ChangeHealth(_fixedStatIncrementer * enemyHealth.GetHealth() / fixedStatDivider);
                Debug.Log(enemy.name + " Health: " + enemyHealth.GetHealth());

                enemyDamage.damage = _fixedStatIncrementer * enemyDamage.damage / fixedStatDivider;
                Debug.Log(enemy.name + " Damage: " + enemyDamage.damage);
            }
        }
    }
}

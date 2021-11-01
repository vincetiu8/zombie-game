using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
	/// <summary>
	///     Handles calculating and scaling enemy stats
	/// </summary>
	[Serializable]
	public class WaveAttributeMultiplier
	{
		[Header("Enemy stat settings")] [Tooltip("Whether or not to increase enemy stats per wave")] [SerializeField]
		private bool increaseStats;

		[Tooltip("Incrementing value used in attribute calculations")] [SerializeField] [Range(0.5f, 1.5f)]
		private float multiplierValue = 1;

		[SerializeField] [Range(0.1f, 1f)] private float randomDeviationMax;

		[Tooltip("How much to increment statIncrementer by. Defaults to 1")] [SerializeField] [Range(0f, 1f)]
		private float multiplierIncrement = 0.1f;

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
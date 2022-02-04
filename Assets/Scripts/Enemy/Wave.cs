using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
	/// <summary>
	///     Represents the current state of the spawner.
	///     <b>Spawning:</b> the spawner is creating enemies.
	///     <b>Waiting:</b> the spawner is waiting until all enemies are dead.
	///     <b>Counting:</b> the spawner is counting down the start of the next wave.
	/// </summary>
	public enum SpawnState
	{
		Spawning,
		Waiting,
		Counting
	}

	/// <summary>
	/// Base class for all waves
	/// </summary>
	[Serializable]
	public abstract class Wave
	{
		public                string           waveName;
		[Range(0, 1f)] public float            spawnDelay;
		public abstract       List<GameObject> GetEnemiesToSpawn();
	}

	/// <summary>
	///     Holds information about a wave.
	///     This is settable in the inspector.
	/// </summary>
	[Serializable]
	public class FixedWave : Wave
	{
		[SerializeField]                private GameObject enemyType;
		[Range(1, 50)] [SerializeField] private int        enemyCount;

		public override List<GameObject> GetEnemiesToSpawn()
		{
			List<GameObject> enemies = new List<GameObject>();

			for (int i = 0; i < enemyCount; i++)
			{
				GameObject spawnedEnemy = enemyType.gameObject;
				enemies.Add(spawnedEnemy);
			}

			return enemies;
		}
	}

	/// <summary>
	///     Holds information about a wave; settable in the inspector.
	///		Contains an array of enemies to be spawned at random
	/// </summary>
	[Serializable]
	public class RandomWave : Wave
	{
		[SerializeField]                private GameObject[] enemyTypes;
		[Range(1, 50)] [SerializeField] private int          enemyCount;

		public override List<GameObject> GetEnemiesToSpawn()
		{
			List<GameObject> enemies = new List<GameObject>();

			for (int i = 0; i < enemyCount; i++)
			{
				int pickedEnemy = Random.Range(0, enemyTypes.Length);

				foreach (GameObject enemy in enemyTypes)
				{
					if (enemyTypes[pickedEnemy] == true)
					{
						GameObject spawnedEnemy = enemyTypes[pickedEnemy];
						enemies.Add(spawnedEnemy);
					}
				}
			}

			return enemies;
		}
	}

	/// <summary>
	/// Behaves similarly to RandomWave, but enemy spawn amounts are
	/// randomly determined
	/// </summary>
	[Serializable]
	public class ChanceWave : Wave
	{
		[SerializeField]                private GameObject[] chanceEnemies;
		[Range(1, 10)] [SerializeField] private int          enemyCountMax;

		public override List<GameObject> GetEnemiesToSpawn()
		{
			List<GameObject> enemies = new List<GameObject>();

			foreach (GameObject enemy in chanceEnemies)
			{
				int enemyAmount = Random.Range(1, enemyCountMax);

				for (int i = 0; i < enemyAmount; i++)
				{
					GameObject spawnedEnemy = enemy.gameObject;
					enemies.Add(spawnedEnemy);
				}
			}

			return enemies;
		}
	}
}
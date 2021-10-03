using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
	/// <summary>
	///     Represents the current state of the spawner.
	///     Spawning: the spawner is creating enemies.
	///     Waiting: the spawner is waiting until all enemies are dead.
	///     Counting: the spawner is counting down the start of the next wave.
	/// </summary>
	public enum SpawnState
	{
		Spawning,
		Waiting,
		Counting
	}

	/// <summary>
	///     Holds information about a wave.
	///     This is settable in the inspector.
	/// </summary>
	[Serializable]
	public struct Wave
	{
		public string     waveName;
		public GameObject enemyType;
		public int        count;
		public float      spawnDelay;
	}

	/// <summary>
	///     Handles spawning waves of enemies.
	/// </summary>
	public class WaveSpawner : MonoBehaviour
	{
		[Header("Wave Information")] [Description("The enemy waves")] [SerializeField]
		private Wave[] waves;

		[Description("The time delay between waves")] [SerializeField] [Range(0, 20)]
		private float waveDelay = 5;

		[Description("The positions where enemies can spawn")] [SerializeField]
		private List<Transform> spawnpoints;

		[Header("Enemy Searching")]
		[Description("How often the number of remaining enemies sholud be checked")]
		[SerializeField]
		[Range(0.1f, 5)]
		private float searchIntervalAmount = 1f;

		private int _nextWaveIndex;

		private float     _searchInterval;
		private Coroutine _spawnCoroutine;

		private SpawnState _state;
		private float      _waveCountdown;

		private void Start()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				enabled = false;
				return;
			}

			_state = SpawnState.Counting;

			if (spawnpoints.Count == 0) Debug.LogError("No available spawnpoints");

			_waveCountdown = waveDelay;
		}

		private void Update()
		{
			switch (_state)
			{
				case SpawnState.Counting:
					_waveCountdown -= Time.deltaTime;

					if (_waveCountdown > 0 || _spawnCoroutine != null) return;

					_spawnCoroutine = StartCoroutine(SpawnWave(waves[_nextWaveIndex]));

					break;
				case SpawnState.Spawning:
					return;
				case SpawnState.Waiting:
				{
					_searchInterval -= Time.deltaTime;

					if (_searchInterval > 0) return;

					_searchInterval = searchIntervalAmount;

					// Don't start the countdown if there are still alive enemies
					if (AreEnemiesAlive()) return;

					StartNewWave();

					break;
				}
				default:
					return;
			}
		}


		/// <summary>
		///     Updates the wave index and countdown
		/// </summary>
		private void StartNewWave()
		{
			_state = SpawnState.Counting;
			_waveCountdown = waveDelay;

			// In case the wave index exceeds the number of waves, we loop back to the start
			_nextWaveIndex += 1 + waves.Length;
			_nextWaveIndex %= waves.Length;
		}

		/// <summary>
		///     Spawns all enemies in a wave
		/// </summary>
		private IEnumerator SpawnWave(Wave wave)
		{
			Debug.Log("Spawning wave: " + wave.waveName);
			_state = SpawnState.Spawning;

			for (int i = 0; i < wave.count; i++)
			{
				SpawnEnemy(wave.enemyType);
				yield return new WaitForSeconds(wave.spawnDelay);
			}

			_state = SpawnState.Waiting;
			_spawnCoroutine = null;
		}

		private void SpawnEnemy(Object enemy)
		{
			// Get a random spawnpoint
			Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];

			PhotonNetwork.Instantiate(enemy.name, spawnpoint.position, Quaternion.identity);
		}

		private static bool AreEnemiesAlive()
		{
			return GameObject.FindGameObjectWithTag("Enemy");
		}

        public void AddSpawnPoints(List<Transform> additionalSpawnPoints)
        {
            // Only adds SpawnPoints that do not already exist to prevent accidentally adding the same points multiple times
            foreach (Transform addedSpawnPoint in additionalSpawnPoints.Where(addedSpawnPoint => !spawnpoints.Contains(addedSpawnPoint)))
            {
                spawnpoints.Add(addedSpawnPoint);
            }
        }
	}
}
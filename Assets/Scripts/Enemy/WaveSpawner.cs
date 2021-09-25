using System;
using System.Collections;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
	#region Related Objects

	// SpawnState holds the current state of the spawner
	// Spawning: the spawner is creating enemies
	// Waiting: the spawner is waiting until all enemies are dead
	// Counting: the spawner is counting down the start of the next wave
	public enum SpawnState
	{
		Spawning,
		Waiting,
		Counting
	}

	// Wave holds information about a wave
	// This is settable in the inspector
	[Serializable]
	public struct Wave
	{
		public string     waveName;
		public GameObject enemyType;
		public int        count;
		public float      spawnDelay;
	}

	#endregion

	public class WaveSpawner : MonoBehaviour
	{
		#region Variables

		[Header("Wave Information")] [Description("The enemy waves")] [SerializeField]
		private Wave[] waves;

		[Description("The time delay between waves")] [SerializeField] [Range(0, 20)]
		private float waveDelay = 5;

		[Description("The positions where enemies can spawn")] [SerializeField]
		private Transform[] spawnpoints;

		// Holds the current state of the spawner
		private SpawnState _state;

		// The time until the next wave starts
		private float _waveCountdown;

		// The index of the next wave
		private int _nextWaveIndex;

		// The coroutine spawning the enemies
		private Coroutine _spawnCoroutine;

		[Header("Enemy Searching")]
		[Description("How often the number of remaining enemies sholud be checked")]
		[SerializeField]
		[Range(0.1f, 5)]
		private float searchIntervalAmount = 1f;

		// The time remaining before the next enemy search
		private float _searchInterval;

		#endregion

		#region Wave Spawner Methods

		// StartNextWave updates the wave index and countdown
		private void StartNewWave()
		{
			_state = SpawnState.Counting;
			_waveCountdown = waveDelay;

			// In case the wave index exceeds the number of waves, we loop back to the start
			_nextWaveIndex += 1 + waves.Length;
			_nextWaveIndex %= waves.Length;
		}

		// SpawnWave spawns all enemies in a wave
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
			Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];

			PhotonNetwork.Instantiate(enemy.name, spawnpoint.position, Quaternion.identity);
		}

		private static bool AreEnemiesAlive()
		{
			return GameObject.FindGameObjectWithTag("Enemy");
		}

		#endregion

		#region Unity Callbacks

		private void Start()
		{
			_state = SpawnState.Counting;

			if (spawnpoints.Length == 0) Debug.LogError("No available spawnpoints");

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

		#endregion
	}
}
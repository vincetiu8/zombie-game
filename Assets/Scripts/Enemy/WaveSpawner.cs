using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	///     Handles spawning waves of enemies.
	/// </summary>
	public class WaveSpawner : MonoBehaviour
	{
		[Header("Enemy Searching")]
		[Tooltip("How often the number of remaining enemies should be checked")]
		[Range(0.1f, 5)]
		[SerializeField]
		private float searchIntervalAmount = 1f;

		[Header("Wave Settings")] [Tooltip("The time delay between waves")] [Range(0, 20)] [SerializeField]
		private float waveDelay = 5;

		[Tooltip("Settings for scaling enemy stats ")] [SerializeReference]
		private WaveAttributeMultiplier attributeMultiplier = new WaveAttributeMultiplier();

		[Tooltip("List of enemy waves")] [SerializeReference]
		public List<Wave> waveList;

		[Tooltip("The positions where enemies can spawn")]
		private readonly List<Transform> _spawnpoints = new List<Transform>();

		private int       _nextWaveIndex;
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
			_waveCountdown = waveDelay;
		}

		private void Update()
		{
			switch (_state)
			{
				case SpawnState.Counting:
					_waveCountdown -= Time.deltaTime;

					if (_waveCountdown > 0 || _spawnCoroutine != null) return;
					_spawnCoroutine = StartCoroutine(SpawnWave(waveList[_nextWaveIndex]));

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
			_nextWaveIndex += 1 + waveList.Count;
			_nextWaveIndex %= waveList.Count;
		}

		/// <summary>
		/// Spawns all enemies in a wave
		/// </summary>
		private IEnumerator SpawnWave(Wave wave)
		{
			_state = SpawnState.Spawning;
			Debug.Log($"Got wave {wave.waveName}, spawning");

			foreach (GameObject enemy in wave.GetEnemiesToSpawn())
			{
				Transform spawnpoint = _spawnpoints[Random.Range(0, _spawnpoints.Count)];
				GameObject spawnedEnemy =
					PhotonNetwork.Instantiate(enemy.name, spawnpoint.position, Quaternion.identity);

				attributeMultiplier.MultiplyEnemyStats(spawnedEnemy);

				yield return new WaitForSeconds(wave.spawnDelay);
			}

			attributeMultiplier.Increment();
			_spawnCoroutine = null;
			_state = SpawnState.Waiting;
		}

		private static bool AreEnemiesAlive()
		{
			return GameObject.FindGameObjectWithTag("Enemy");
		}

		public void AddSpawnPoints(IEnumerable<Transform> additionalSpawnPoints)
		{
			// Only adds SpawnPoints that do not already exist to prevent accidentally adding the same points multiple times
			foreach (Transform addedSpawnPoint in additionalSpawnPoints.Where(addedSpawnPoint =>
				                                                                  !_spawnpoints
					                                                                  .Contains(addedSpawnPoint)))
			{
				_spawnpoints.Add(addedSpawnPoint);
			}
		}
	}
}
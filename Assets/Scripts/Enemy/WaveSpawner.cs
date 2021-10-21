using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Networking;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
	/// <summary>
	///     Handles spawning waves of enemies.
	/// </summary>
	public class WaveSpawner : MonoBehaviour
	{
		[Header("Enemy Searching")]
		[Tooltip("How often the number of remaining enemies should be checked")]
		[Range(0.1f, 5)] [SerializeField]
		private float searchIntervalAmount = 1f;
		
		[Header("Wave Settings")]
		[Tooltip("The time delay between waves")] 
		[Range(0, 20)] [SerializeField]
		private float waveDelay = 5;
		
		[Tooltip("The positions where enemies can spawn")] [SerializeField]
		private List<Transform> spawnpoints = new List<Transform>();
		
		[SerializeReference] public List<Wave> waveList;

		private int _nextWaveIndex;
		private float _searchInterval;
		private float _waveCountdown;

		private SpawnState _state;
		private WaveAttributeMultiplier _attributeMultiplier;

		private void Start()
		{
			_attributeMultiplier = GetComponent<WaveAttributeMultiplier>();
			
			if (!PhotonNetwork.IsMasterClient)
			{
				enabled = false;
				return;
			}

			_state = SpawnState.Counting;

			_waveCountdown = waveDelay;

			_attributeMultiplier.statIncrementer = _attributeMultiplier.incrementerBase;

			if (!_attributeMultiplier.fixedMultiplier)
			{
				Debug.Log("Using random multipliers");
				return;
			}

			Debug.Log("Using fixed multipliers");
		}

		private void Update()
		{
			switch (_state)
			{
				case SpawnState.Counting:
					_waveCountdown -= Time.deltaTime;

					if (_waveCountdown > 0) return;

					SpawnWave(waveList[_nextWaveIndex]);
					
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

			if (_attributeMultiplier.resetStatIncrease)
			{
				if (_attributeMultiplier.statIncrementer > waveList.Count)
				{
					_attributeMultiplier.statIncrementer = _attributeMultiplier.incrementerBase;
					Debug.Log("reset incrementer");
				}
			}
			
			// In case the wave index exceeds the number of waves, we loop back to the start
			_nextWaveIndex += 1 + waveList.Count;
			_nextWaveIndex %= waveList.Count;
		}

		/// <summary>
		/// Spawns all enemies in a ChanceWave
		/// </summary>
		private void SpawnWave(Wave wave)
		{
			_state = SpawnState.Spawning;

			foreach (var enemy in wave.GetEnemiesToSpawn())
			{
				Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
				GameObject spawnedEnemy = PhotonNetwork.Instantiate(enemy.name, spawnpoint.position, Quaternion.identity);
				
				_attributeMultiplier.CalculateEnemyStats(spawnedEnemy);
			}

			if (!_attributeMultiplier.fixedMultiplier)
			{
				_attributeMultiplier.randomDeviationMin += _attributeMultiplier.statIncrement;
				_attributeMultiplier.randomDeviationMax += _attributeMultiplier.statIncrement;
			}
			_attributeMultiplier.statIncrementer += _attributeMultiplier.statIncrement;

			_state = SpawnState.Waiting;
		}
		
		private static bool AreEnemiesAlive()
		{
			return GameObject.FindGameObjectWithTag("Enemy");
		}

		public  void AddSpawnPoints(IEnumerable<Transform> additionalSpawnPoints)
		{
			// Only adds SpawnPoints that do not already exist to prevent accidentally adding the same points multiple times
			foreach (Transform addedSpawnPoint in additionalSpawnPoints.Where(addedSpawnPoint => !spawnpoints.Contains(addedSpawnPoint)))
			{
				spawnpoints.Add(addedSpawnPoint);
			}
		}
	}
}

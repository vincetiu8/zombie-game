using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
		[Header("Wave Settings")]
		[Tooltip("The time delay between waves")] [Range(0, 20)]
		public float waveDelay = 5;

		[Header("Enemy Searching")]
		[Tooltip("How often the number of remaining enemies should be checked")]
		[Range(0.1f, 5)]
		public float searchIntervalAmount = 1f;
		
		[SerializeReference] public List<Wave> waveList;

		private int _nextWaveIndex;

		private float _searchInterval;
		private Coroutine _spawnCoroutine;

		private SpawnState _state;
		private WaveAttributeMultiplier _attributeMultiplier;
		private float _waveCountdown;

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

					if (_waveCountdown > 0 || _spawnCoroutine != null) return;

					foreach (var wave in waveList)
					{
						if (wave is FixedWave)
						{
							// Error here because Wave is passed in instead of FixedWave
							// Need a better way to do this 
							_spawnCoroutine = StartCoroutine(SpawnWave(waveList[_nextWaveIndex]));
						}
					}
					
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
		/// Spawns all enemies in a Wave
		/// </summary>
		private IEnumerator SpawnWave(FixedWave wave)
		{
			Debug.Log("Spawning wave: " + wave.waveName);
			_state = SpawnState.Spawning;

			for (int i = 0; i < wave.enemyCount; i++)
			{
				wave.SpawnEnemy();
				yield return new WaitForSeconds(wave.spawnDelay);
			}

			if (!_attributeMultiplier.fixedMultiplier)
			{
				_attributeMultiplier.randomDeviationMin += _attributeMultiplier.fixedStatIncrement;
				_attributeMultiplier.randomDeviationMax += _attributeMultiplier.fixedStatIncrement;
			}
			_attributeMultiplier.statIncrementer += _attributeMultiplier.fixedStatIncrement;

			_state = SpawnState.Waiting;
			_spawnCoroutine = null;
		}

		/// <summary>
		/// Spawns all enemies in a RandomWave
		/// </summary>
		private IEnumerator SpawnWave(RandomWave wave)
		{
			Debug.Log("Spawning wave (random): " + wave.waveName);
			_state = SpawnState.Spawning;

			for (int i = 0; i < wave.enemyCount; i++)
			{
				wave.SpawnEnemy();
				yield return new WaitForSeconds(wave.spawnDelay);
			}

			if (!_attributeMultiplier.fixedMultiplier)
			{
				_attributeMultiplier.randomDeviationMin += _attributeMultiplier.fixedStatIncrement;
				_attributeMultiplier.randomDeviationMax += _attributeMultiplier.fixedStatIncrement;
			}
			_attributeMultiplier.statIncrementer += _attributeMultiplier.fixedStatIncrement;

			_state = SpawnState.Waiting;
			_spawnCoroutine = null;
		}
		
		/// <summary>
		/// Spawns all enemies in a ChanceWave
		/// </summary>
		private IEnumerator SpawnWave(ChanceWave wave)
		{
			Debug.Log("Spawning wave (random): " + wave.waveName);
			_state = SpawnState.Spawning;

			wave.SpawnEnemy();
			yield return new WaitForSeconds(wave.spawnDelay);

			if (!_attributeMultiplier.fixedMultiplier)
			{
				_attributeMultiplier.randomDeviationMin += _attributeMultiplier.fixedStatIncrement;
				_attributeMultiplier.randomDeviationMax += _attributeMultiplier.fixedStatIncrement;
			}
			_attributeMultiplier.statIncrementer += _attributeMultiplier.fixedStatIncrement;

			_state = SpawnState.Waiting;
			_spawnCoroutine = null;
		}
		
		private static bool AreEnemiesAlive()
		{
			return GameObject.FindGameObjectWithTag("Enemy");
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Networking;
using UnityEngine;
using Photon.Pun;
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

    public enum WaveType
    {
        Fixed,
        Random,
        Chance
    }
    
    /// <summary>
    /// Base class for all waves
    /// </summary>
    public class Wave
    {
        [SerializeField] protected List<Transform> spawnpoints = new List<Transform>();

        public void AddSpawnPoints(IEnumerable<Transform> additionalSpawnPoints)
        {
            // Only adds SpawnPoints that do not already exist to prevent accidentally adding the same points multiple times
            foreach (Transform addedSpawnPoint in additionalSpawnPoints.Where(addedSpawnPoint => !spawnpoints.Contains(addedSpawnPoint)))
            {
                spawnpoints.Add(addedSpawnPoint);
            }
        }
    }
    
    /// <summary>
    ///     Holds information about a wave.
    ///     This is settable in the inspector.
    /// </summary>
    [Serializable]
    public class FixedWave : Wave
    {
        public string     waveName;
        public GameObject enemyType;
        public int        enemyCount;
        public float      spawnDelay;

        public void SpawnEnemy(GameObject enemy)
        {
            // Get a random spawnpoint
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
            
            GameObject spawnedEnemy = PhotonNetwork.Instantiate(enemy.name, spawnpoint.position, Quaternion.identity);

            WaveAttributeMultiplier attributeMultiplier = GameManager.Instance.GetComponent<WaveAttributeMultiplier>();
            attributeMultiplier.CalculateEnemyStats(spawnedEnemy);
        }
    }
	
    /// <summary>
    ///     Holds information about a wave; settable in the inspector.
    ///		Contains an array of enemies to be spawned at random
    /// </summary>
    [Serializable]
    public class RandomWave : Wave
    {
        public string     waveName;
        public GameObject[] enemyTypes;
        public int        enemyCount;
        public float      spawnDelay;

        public void SpawnEnemy(RandomWave wave)
        {
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];

            int pickedEnemy = Random.Range(0, wave.enemyTypes.Length);

            foreach (GameObject enemy in wave.enemyTypes)
            {
                if (wave.enemyTypes[pickedEnemy] == true)
                {
                    GameObject spawnedEnemy = PhotonNetwork.Instantiate(wave.enemyTypes[pickedEnemy].name,
                        spawnpoint.position, Quaternion.identity);

                    WaveAttributeMultiplier attributeMultiplier =
                        GameManager.Instance.GetComponent<WaveAttributeMultiplier>();
                    attributeMultiplier.CalculateEnemyStats(spawnedEnemy);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Behaves similarly to RandomWave, but enemy spawn amounts are r
    /// randomly determined
    /// </summary>
    [Serializable]
    public class ChanceWave : Wave
    {
        public string waveName;
        public GameObject[] chanceEnemies;
        [Range(1, 10)] public int enemyChanceMax;
        public int enemyCount;
        public float spawnDelay;

        public void SpawnEnemy(ChanceWave wave)
        {
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
            
            int enemyAmount = Random.Range(1, enemyChanceMax);

            foreach (GameObject enemy in wave.chanceEnemies)
            {
                for (int i = 0; i < enemyAmount; i++)
                {
                    GameObject spawnedEnemy = PhotonNetwork.Instantiate(enemy.name,
                        spawnpoint.position, Quaternion.identity);

                    WaveAttributeMultiplier attributeMultiplier =
                        GameManager.Instance.GetComponent<WaveAttributeMultiplier>();
                    attributeMultiplier.CalculateEnemyStats(spawnedEnemy);
                }
            }
        }
    }

}


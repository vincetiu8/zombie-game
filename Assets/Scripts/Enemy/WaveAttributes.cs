using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    /// <summary>
    ///     Holds information about a wave.
    ///     This is settable in the inspector.
    /// </summary>
    [Serializable]
    public class FixedWave
    {
        public string     waveName;
        public GameObject enemyType;
        public int        enemyCount;
        public float      spawnDelay;
        public Transform[] spawnpoints;
        
        public void SpawnEnemy(GameObject enemy)
        {
            // Get a random spawnpoint
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            
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
    public struct RandomWave
    {
        public string     waveName;
        public GameObject[] enemyTypes;
        public int        enemyCount;
        public float      spawnDelay;
        public Transform[] spawnpoints;
        
        public void SpawnEnemy(RandomWave wave)
        {
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];

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
    public struct ChanceWave
    {
        public string waveName;
        public GameObject[] chanceEnemies;
        [Range(1, 10)] public int enemyChanceMax;
        public int enemyCount;
        public float spawnDelay;
        public Transform[] spawnpoints;

        public void SpawnEnemy(ChanceWave wave)
        {
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            
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


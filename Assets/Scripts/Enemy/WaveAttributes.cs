using System;
using System.ComponentModel;
using UnityEngine;

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
    public class FixedWave : Wave
    {
        [SerializeField] public string     waveName;
        public GameObject enemyType;
        public int        enemyCount;
        public float      spawnDelay;
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
    }

}


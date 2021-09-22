using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class WaveSpawner : MonoBehaviour
    {   
        //will probably move this enum and the wave class to a separate script in the future
        public enum SpawnState {Spawning, Waiting, Counting}

        //will probably make this a struct in the future
        [Serializable]
        public class Wave 
        {
            public string waveName;
            public Transform enemyType;
            public int count;
            public float spawnRate;
        }

        public Wave[] waves;
        private int _nextWave = 0;
        
        //time between waves
        public float waveDelay = 5;
        public float waveCoutndown;

        private SpawnState state = SpawnState.Counting;

        private void Start()
        {
            waveCoutndown = waveDelay;
        }

        private void Update()
        {
            if(waveCoutndown <= 0 && state != SpawnState.Spawning)
            {
                //spawn wave
            } 
            else
            {
                waveCoutndown -= Time.deltaTime;
            }
        }
    }

}
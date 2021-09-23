using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;

namespace Spawners
{
    public class WaveSpawner : MonoBehaviour
    {   
        //todo: will probably move this enum and the wave struct to a separate script in the future
        public enum SpawnState {Spawning, Waiting, Counting}

        [Serializable]
        public struct Wave 
        {
            public string waveName;
            public GameObject enemyType;
            public int count;
            public float spawnRate;
        }

        [SerializeField] private Wave[] waves;
        [SerializeField] private Transform[] spawnpoints;
        private SpawnState _state;
        private Coroutine _spawnCoroutine;

        //time between waves
        [SerializeField] private float waveDelay = 5;
        private float _waveCoutndown;
        private int _nextWave = 0;

        private float _searchInterval;
        [SerializeField] private float searchIntervalAmount = 1f;
        
        private void Start()
        {   
            _state = SpawnState.Counting;
            if(spawnpoints.Length == 0)
            {
                Debug.LogError("No available spawnpoints");
            }
            
            _waveCoutndown = waveDelay;

            if(_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }
        }

        private void Update()
        {
            if(_state == SpawnState.Counting)
            {
                _waveCoutndown -= Time.deltaTime;
                _searchInterval -= Time.deltaTime;  
            }

            if(_state == SpawnState.Spawning)
            {
                return;
            }

            if(_state == SpawnState.Waiting && _searchInterval <= 0f)
            {
                //check for alive enemies
                if(!AreEnemiesAlive())
                {
                    //new wave
                    _searchInterval = searchIntervalAmount;
                    StartNewWave();
                }
                return; 
            }

            if(_waveCoutndown <= 0 && _state != SpawnState.Spawning)
            {
                //spawn wave
                _spawnCoroutine = StartCoroutine(SpawnWave(waves[_nextWave]));
                return;
            }          
        }

        private void StartNewWave()
        {
            Debug.Log("wave completed, looping");

            _state = SpawnState.Counting;
            _waveCoutndown = waveDelay;
            
            _nextWave += 1 + waves.Length;
            _nextWave %= waves.Length;

        }

        //do i need to assign this to some sort of private Coroutine variable?
        private IEnumerator SpawnWave(Wave wave)
        {
            Debug.Log("Spawning wave: " + wave.waveName);
            _state = SpawnState.Spawning;

            for (int i = 0; i < wave.count; i++)
            {
                SpawnEnemy(wave.enemyType);
                yield return new WaitForSeconds(1/wave.spawnRate);
            }

            _state = SpawnState.Waiting;

            yield break;
        }   

        private void SpawnEnemy(GameObject enemy)
        {
            Debug.Log("Spawning enemy: " + enemy.name);
            Transform spawnpoint = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Length)];
            PhotonNetwork.Instantiate(enemy.name, spawnpoint.position, Quaternion.identity);
        }

        private bool AreEnemiesAlive()
        {
            return GameObject.FindGameObjectWithTag("Enemy");
        }
    }

}
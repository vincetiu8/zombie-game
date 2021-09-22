using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Spawners
{
    public class WaveSpawner : MonoBehaviour
    {   
        //will probably move this enum and the wave struct to a separate script in the future
        public enum SpawnState {Spawning, Waiting, Counting}

        [Serializable]
        public struct Wave 
        {
            public string waveName;
            public Transform enemyType;
            public int count;
            public float spawnRate;
        }

        [SerializeField] private Wave[] waves;
        [SerializeField] private Transform[] spawnpoints;
        private int _nextWave = 0;
        
        //time between waves
        [SerializeField] private float waveDelay = 5;
        private float waveCoutndown;

        private SpawnState state = SpawnState.Counting;

        private float _searchInterval = 1f;

        private void Start()
        {
            if(spawnpoints.Length == 0)
            {
                Debug.LogError("No available spawnpoints");
            }
            
            waveCoutndown = waveDelay;
        }

        private void Update()
        {
            if(state == SpawnState.Waiting)
            {
                //check for alive enemies
                if(!EnemyIsAlive())
                {
                    //new wave
                    StartNewWave();
                }
                else
                {
                    return;
                }
            }

            if(waveCoutndown <= 0 && state != SpawnState.Spawning)
            {
                //spawn wave
                StartCoroutine(SpawnWave(waves[_nextWave]));
                return;
            } 
            else
            {
                waveCoutndown -= Time.deltaTime;
            }
        }

        private void StartNewWave()
        {
            Debug.Log("wave completed");

            state = SpawnState.Counting;
            waveCoutndown = waveDelay;
            
            if(_nextWave + 1 > waves.Length - 1)
            {
                //resets wave counter
                _nextWave = 0;
                Debug.Log("completed all waves, looping");
            }
            else
            {
                _nextWave++;
            }

        }

        //do i need to assign this to some sort of private Coroutine variable?
        private IEnumerator SpawnWave(Wave _wave)
        {
            Debug.Log("Spawning wave: " + _wave.waveName);
            state = SpawnState.Spawning;

            for (int i = 0; i < _wave.count; i++)
            {
                SpawnEnemy(_wave.enemyType);
                yield return new WaitForSeconds(1/_wave.spawnRate);
            }

            state = SpawnState.Waiting;

            yield break;
        }   

        private void SpawnEnemy(Transform _enemy)
        {
            Debug.Log("Spawning enemy: " + _enemy.name);
            Transform _spawnpoint = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Length)];
            PhotonNetwork.Instantiate(_enemy.name, _spawnpoint.position, Quaternion.identity);
        }

        private bool EnemyIsAlive()
        {
            _searchInterval -= Time.deltaTime;
            if(_searchInterval <= 0f)
            {
                _searchInterval = 1f;
                if(GameObject.FindWithTag("Enemy") == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

}
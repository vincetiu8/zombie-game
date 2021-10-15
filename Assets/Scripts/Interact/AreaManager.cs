using System;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Networking;
using UnityEngine;

namespace Interact
{
    public class AreaManager  : MonoBehaviour
    {
        private WaveSpawner _waveSpawner;
        //private Transform enemySpawnpoint;
        private List<Transform> _rooms;

        private void Start()
        {
            _waveSpawner = GameManager.Instance.GetComponent<WaveSpawner>();

            _rooms = transform.Cast<Transform>().ToList();
            
            //enemySpawnpoint = transform.Find("Enemy Spawnpoints");
        }

        public void UnlockRoom(string roomName)
        {
            Transform room = _rooms.SingleOrDefault(spawn => spawn.name == roomName);
            if (room == null) return;
            
            List<Transform> enemySpawnPoints = room.Find("Enemy Spawnpoint").Cast<Transform>().ToList();
            _waveSpawner.AddSpawnPoints(enemySpawnPoints);
            
            _rooms.Remove(room);
        }
    }
}

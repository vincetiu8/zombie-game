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
        private List<Transform> _rooms;

        private void Start()
        {
            _waveSpawner = GameManager.Instance.GetComponent<WaveSpawner>();

            // All children of the gameObject should be rooms
            _rooms = transform.Cast<Transform>().ToList();
        }

        public void UnlockRoom(string roomName)
        {
            // Find room by it's name
            Transform room = _rooms.SingleOrDefault(spawn => spawn.name == roomName);
            if (room == null) return;
            
            // Compile enemy spawnpoints into a list to add to wavespawner
            List<Transform> enemySpawnPoints = room.Find("Enemy Spawnpoints").Cast<Transform>().ToList();
            _waveSpawner.AddSpawnPoints(enemySpawnPoints);
            Debug.Log("Unlocked: " + room);
            
            // Removes unlocked room from list so it can't be unlocked again
            _rooms.Remove(room);
        }
    }
}

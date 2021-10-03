using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Enemy;

namespace Interact
{
    public class UnlockableAreas : MonoBehaviour
    {
        [SerializeField] private WaveSpawner waveSpawner;
        
        /// <summary>
        /// Sets all objects in the area active and add all enemy spawn points to to the WaveSpawner
        /// </summary>
        /// <param name="areaName"> Should be the same as the name used for the door text</param>
        public void UnlockArea(string areaName)
        {
            Transform room = transform.Find(areaName);
            if (!room)
            {
                Debug.Log("No room with this name exists");
                return;
            }
            // Will be inactive by default since they're unused until the room is opened
            room.Find("Objects").gameObject.SetActive(true);
            
            Transform addEnemySpawns = room.Find("Enemy");
            
            // Should be active by default already, but this is just in case
            addEnemySpawns.gameObject.SetActive(true);
            
            // Take all the transforms of the children found in Enemy into a list
            List<Transform> enemySpawnPoint = addEnemySpawns.Cast<Transform>().ToList();

            waveSpawner.AddSpawnPoints(enemySpawnPoint);
        }
    }
}

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
        /// Rooms should have only 2 children, "Enemy" and "Objects"
        /// "Enemy" will contain all the enemy spawn points (Set active true in the inspector)
        /// "Objects" will contain the objects to be set active (Set active false in the inspector)
        /// </summary>
        /// <param name="areaName"> Should be the same as the room name assigned at UnlockableAreas gameobject</param>
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
            
            // Take all the transforms of the children found in "Enemy" into a list
            List<Transform> enemySpawnPoint = room.Find("Enemy").Cast<Transform>().ToList();

            waveSpawner.AddSpawnPoints(enemySpawnPoint);
        }
    }
}

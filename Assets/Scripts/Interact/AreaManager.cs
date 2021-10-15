using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Networking;
using UnityEngine;

namespace Interact
{
    [Serializable]
    public class RoomDict : SerializableDictionary<string, List<Transform>>
    {
    }
    
    [ExecuteInEditMode]
    public class AreaManager  : MonoBehaviour
    {
        private WaveSpawner _waveSpawner;
        [SerializeField] private Transform _areaManager;

        [SerializeField] private RoomDict roomNameTransfroms;
        
        private void Start()
        {
            foreach (Transform room in _areaManager) roomNameTransfroms.Add(room.name, room.Find("Enemy Spawnpoints").Cast<Transform>().ToList());
            StartCoroutine(LateStart());
        }
        
        // late start here acts like the normal start
        private IEnumerator LateStart()
        {
            yield return new WaitUntil(() => Application.isPlaying);
            _waveSpawner = GameManager.Instance.GetComponent<WaveSpawner>();
        }

        public void UnlockRoom(string roomName)
        {
            if (!roomNameTransfroms.ContainsKey(roomName)) return;
            _waveSpawner.AddSpawnPoints(roomNameTransfroms[roomName]);
        }
    }
}

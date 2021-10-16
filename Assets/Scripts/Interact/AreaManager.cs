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
        [SerializeField] private Transform _areaManager;
        
        private Dictionary<string, List<Transform>> roomNameTransfroms = new Dictionary<string, List<Transform>>();

        private void Start()
        {
            foreach (Transform room in _areaManager) roomNameTransfroms.Add(room.name, room.Find("Enemy Spawnpoints").Cast<Transform>().ToList());
            _waveSpawner = GameManager.Instance.GetComponent<WaveSpawner>();
            UnlockRoom("Spawn Room");
        }

        public void UnlockRoom(string roomName)
        {
            if (!roomNameTransfroms.ContainsKey(roomName)) return;
            _waveSpawner.AddSpawnPoints(roomNameTransfroms[roomName]);
        }
    }
}

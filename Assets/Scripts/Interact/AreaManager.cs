using System.Collections.Generic;
using System.Linq;
using Enemy;
using Networking;
using UnityEngine;

namespace Interact
{
	public class AreaManager : MonoBehaviour
	{
		[SerializeField] private Transform areaTransforms;

		private Dictionary<string, List<Transform>> _roomNameTransforms;
		private WaveSpawner                         _waveSpawner;

		private void Start()
		{
			_roomNameTransforms = new Dictionary<string, List<Transform>>();
			foreach (Transform room in areaTransforms)
			{
				_roomNameTransforms.Add(room.name, room.Find("Enemy Spawnpoints")
				                                       .Cast<Transform>().ToList());
			}

			_waveSpawner = GameManager.Instance.GetComponent<WaveSpawner>();
			UnlockRoom("Spawn Room");
		}

		public void UnlockRoom(string roomName)
		{
			if (!_roomNameTransforms.ContainsKey(roomName)) return;
			_waveSpawner.AddSpawnPoints(_roomNameTransforms[roomName]);
		}
	}
}
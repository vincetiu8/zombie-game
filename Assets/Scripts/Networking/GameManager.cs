using Photon.Pun;
using Shop;
using UnityEngine;

namespace Networking
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public static GameManager instance;

		[HideInInspector] public GameObject localPlayer;

		[SerializeField] private GameObject  playerPrefab;
		[SerializeField] private Transform[] spawnpoints;

		private void Awake()
		{
			if (instance != null) Destroy(this);

			instance = this;
		}

		private void Start()
		{
			// Selecting spawn randomly from a range
			// todo: once rooms are implemented, have the spawn predetermined by the player's room number
			Debug.Log("Spawning Player!");
			int position = Random.Range(0, spawnpoints.Length);
			Vector3 spawnPosition = spawnpoints[position].position;
			localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
			localPlayer.GetComponent<PlayerGold>().goldSystem = gameObject.GetComponent<GoldSystem>();
		}
	}
}
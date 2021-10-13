using System;
using Photon.Pun;
using Shop;
using UnityEngine;

namespace Networking
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public static GameManager instance;

		[HideInInspector] public GoldSystem   goldSystem;
		[HideInInspector] public GameObject[] playerInstances;
		[HideInInspector] public GameObject   localPlayerInstance;

		[SerializeField] private GameObject  playerPrefab;
		[SerializeField] private Transform[] spawnpoints;

		private void Awake()
		{
			if (instance != null) Destroy(this);

			instance = this;
			playerInstances = Array.Empty<GameObject>();
			goldSystem = GetComponentInChildren<GoldSystem>();
		}

		private void Start()
		{
			// Player numbers start indexing at 1, need to correct for array
			int correctedPlayerNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;
			Vector3 spawnPosition = spawnpoints[correctedPlayerNumber].position;
			localPlayerInstance = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
		}

		public void SetPlayerInstance(int playerNumber, GameObject player)
		{
			if (playerNumber < 0) Debug.LogError("Attempting to add player instance with negative player number");

			if (playerNumber >= playerInstances.Length) Array.Resize(ref playerInstances, playerNumber + 1);

			playerInstances[playerNumber] = player;
		}

		public GameObject GetPlayerInstance(int playerNumber)
		{
			return playerInstances[playerNumber];
		}
	}
}
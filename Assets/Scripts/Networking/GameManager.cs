using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Shop;
using UnityEngine;

namespace Networking
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public static GameManager Instance;

		[HideInInspector] public GoldSystem   goldSystem;
		[HideInInspector] public GameObject[] playerInstances;
		[HideInInspector] public GameObject   localPlayerInstance;

		[SerializeField] private GameObject  playerPrefab;
		[SerializeField] private Transform[] spawnpoints;

		private SpectatorManager _spectatorManager;

		private void Awake()
		{
			if (Instance != null) Destroy(this);

			Instance = this;
			playerInstances = Array.Empty<GameObject>();
			goldSystem = GetComponentInChildren<GoldSystem>();
			_spectatorManager = GetComponent<SpectatorManager>();
		}

		private void Start()
			{
				int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
			Vector3 spawnPosition = spawnpoints[playerNumber].position;
			localPlayerInstance = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
		}

		public void SetPlayerInstance(int playerNumber, GameObject player)
		{
			if (playerNumber < 0) Debug.LogError("Attempting to add player instance with negative player number");

			if (playerNumber >= playerInstances.Length) Array.Resize(ref playerInstances, playerNumber + 1);

			playerInstances[playerNumber] = player;
		}

		public void RemovePlayerInstance(int playerNumber)
		{
			playerInstances[playerNumber] = null;
			_spectatorManager.OnPlayerDeath(playerNumber);
		}
	}
}
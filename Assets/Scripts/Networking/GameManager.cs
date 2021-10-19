using System.Collections.Generic;
using Menus;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Shop;
using UnityEngine;

namespace Networking
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public static            GameManager      Instance;
		[HideInInspector] public GoldSystem       goldSystem;
		[HideInInspector] public GameObject       localPlayerInstance;
		[HideInInspector] public SpectatorManager spectatorManager;

		[SerializeField] private GameObject  playerPrefab;
		[SerializeField] private Transform[] spawnpoints;
		[SerializeField] private MenuManager menuManager;

		public Dictionary<int, GameObject> PlayerInstances;


		private void Awake()
		{
			if (Instance != null) Destroy(this);

			Instance = this;
			PlayerInstances = new Dictionary<int, GameObject>();
			goldSystem = GetComponentInChildren<GoldSystem>();
			spectatorManager = GetComponent<SpectatorManager>();
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

			PlayerInstances[playerNumber] = player;
		}

		public void RemovePlayerInstance(int playerNumber)
		{
			PlayerInstances.Remove(playerNumber);

			if (PlayerInstances.Count != 0) return;

			menuManager.OpenMenu("death");
		}
	}
}
using Photon.Pun;
using Shop;
using UnityEngine;

namespace Networking
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public static GameManager instance;

		public GoldSystem goldSystem;

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
			goldSystem = GetComponent<GoldSystem>();

			// Player numbers start indexing at 1, need to correct for array
			int correctedPlayerNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;
			Vector3 spawnPosition = spawnpoints[correctedPlayerNumber].position;
			localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
		}
	}
}
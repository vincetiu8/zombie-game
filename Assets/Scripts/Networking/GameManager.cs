using Photon.Pun;
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
			Vector3 spawnPosition = spawnpoints[PhotonNetwork.LocalPlayer.ActorNumber].position;
			localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
		}
	}
}
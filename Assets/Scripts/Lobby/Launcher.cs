using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lobby
{
	// Launcher handles connecting to rooms and loading the game
	public class Launcher : MonoBehaviourPunCallbacks
	{
		#region Variables

		public static Launcher instance;

		[Header("UI Objects")] [SerializeField]
		private TMP_InputField roomNameInputField;

		[SerializeField] private TMP_Text   errorText;
		[SerializeField] private TMP_Text   roomNameText;
		[SerializeField] private Transform  roomListContent;
		[SerializeField] private GameObject roomListItemPrefab;
		[SerializeField] private Transform  playerListContent;
		[SerializeField] private GameObject playerListItemPrefab;
		[SerializeField] private GameObject startGameButton;

		#endregion

		#region Unity Callbacks

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			Debug.Log("Connecting to Master");
			PhotonNetwork.ConnectUsingSettings();
		}

		#endregion

		#region PUN Callbacks

		public override void OnConnectedToMaster()
		{
			Debug.Log("Connected to Master");
			PhotonNetwork.JoinLobby();
			PhotonNetwork.AutomaticallySyncScene = true;
		}

		public override void OnJoinedLobby()
		{
			MenuManager.instance.OpenMenu("title");
			Debug.Log("Joined Lobby");
			PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
		}

		public override void OnJoinedRoom()
		{
			MenuManager.instance.OpenMenu("room");
			roomNameText.text = PhotonNetwork.CurrentRoom.Name;

			Player[] players = PhotonNetwork.PlayerList;

			foreach (Transform child in playerListContent)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < players.Count(); i++)
			{
				Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
			}

			startGameButton.SetActive(PhotonNetwork.IsMasterClient);
		}

		public override void OnMasterClientSwitched(Player newMasterClient)
		{
			startGameButton.SetActive(PhotonNetwork.IsMasterClient);
		}

		public override void OnCreateRoomFailed(short returnCode, string message)
		{
			errorText.text = "Room Creation Failed: " + message;
			Debug.LogError("Room Creation Failed: " + message);
			MenuManager.instance.OpenMenu("error");
		}

		public override void OnLeftRoom()
		{
			MenuManager.instance.OpenMenu("title");
		}

		public override void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			foreach (Transform trans in roomListContent)
			{
				Destroy(trans.gameObject);
			}

			foreach (RoomInfo room in roomList)
			{
				if (room.RemovedFromList) continue;
				Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(room);
			}
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
		}

		#endregion

		#region Public Methods

		public void CreateRoom()
		{
			if (string.IsNullOrEmpty(roomNameInputField.text))
			{
				return;
			}

			PhotonNetwork.CreateRoom(roomNameInputField.text);
			MenuManager.instance.OpenMenu("loading");
		}

		public void StartGame()
		{
			PhotonNetwork.LoadLevel(1);
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
			MenuManager.instance.OpenMenu("loading");
		}

		public void JoinRoom(RoomInfo info)
		{
			PhotonNetwork.JoinRoom(info.Name);
			MenuManager.instance.OpenMenu("loading");
		}

		#endregion
	}
}
using System.Collections.Generic;
using System.Linq;
using Menus;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Lobby
{
	// Launcher handles connecting to rooms and loading the game
	public class Launcher : MonoBehaviourPunCallbacks
	{
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
		[SerializeField] private GameObject errorRoomText;

		private bool _loadingRoom;

		private void Awake()
		{
			instance = this;
			_loadingRoom = false;
		}

		private void Start()
		{
			Debug.Log("Connecting to Master");
			PhotonNetwork.ConnectUsingSettings();
		}

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
			MenuManager.instance.OpenMenu("roommenu");
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
			Debug.Log($"Unable to create room: {message}");

			Debug.Log(returnCode);

			if (returnCode != 32766) return;

			// Room name taken
			MenuManager.instance.OpenMenu("createroom");
			errorRoomText.SetActive(true);
		}

		public override void OnLeftRoom()
		{
			MenuManager.instance.OpenMenu("title");
		}

		public override void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			Debug.Log("Updating rooms...");

			foreach (RoomInfo room in roomList)
			{
				bool found = false;
				foreach (Transform trans in roomListContent)
				{
					RoomListItem transRoom = trans.GetComponent<RoomListItem>();
					if (transRoom.RoomInfo.Name != room.Name) continue;

					if (room.RemovedFromList) Destroy(trans);

					found = true;
					break;
				}

				if (room.RemovedFromList || found) continue;

				GameObject roomInstance = Instantiate(roomListItemPrefab, roomListContent);
				roomInstance.GetComponent<RoomListItem>().Setup(room);
			}
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
		}

		public void CreateRoom()
		{
			if (string.IsNullOrEmpty(roomNameInputField.text))
			{
				return;
			}

			Debug.Log($"Attempting to create room with name {roomNameInputField.text}");
			PhotonNetwork.CreateRoom(roomNameInputField.text);
			MenuManager.instance.OpenMenu("loading");
		}

		public void StartGame()
		{
			if (_loadingRoom) return;
			_loadingRoom = true;
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

		public void Quit()
		{
			// save any game data here
#if UNITY_EDITOR
			// Application.Quit() does not work in the editor so
			// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
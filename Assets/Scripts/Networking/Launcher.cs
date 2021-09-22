using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Networking
{
	public class Launcher : MonoBehaviourPunCallbacks
	{
		#region Variables
		public static Launcher Instance;

		[SerializeField] private TMP_InputField roomNameInputField;
		[SerializeField] private TMP_Text errorText;
		[SerializeField] private TMP_Text roomNameText;
		[SerializeField] private Transform roomListContent;
		[SerializeField] private GameObject roomListItemPrefab;
		[SerializeField] private Transform playerListContent;
		[SerializeField] private GameObject playerListItemPrefab;
		[SerializeField] private GameObject startGameButton;
		
		#endregion

		#region Unity Method
		void Awake()
		{
			Instance = this;
		}

		void Start()
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
			MenuManager.Instance.OpenMenu("title");
			Debug.Log("Joined Lobby");
			PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
		}
		public override void OnJoinedRoom()
		{
			MenuManager.Instance.OpenMenu("room");
			roomNameText.text = PhotonNetwork.CurrentRoom.Name;

			Player[] players = PhotonNetwork.PlayerList;

			foreach(Transform child in playerListContent)
			{
				Destroy(child.gameObject);
			}

			for(int i = 0; i < players.Count(); i++)
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
			MenuManager.Instance.OpenMenu("error");
		}
		public override void OnLeftRoom()
		{
			MenuManager.Instance.OpenMenu("title");
		}
		public override void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			foreach(Transform trans in roomListContent)
			{
				Destroy(trans.gameObject);
			}

			for(int i = 0; i < roomList.Count; i++)
			{
				if(roomList[i].RemovedFromList)
					continue;
				Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
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
			if(string.IsNullOrEmpty(roomNameInputField.text))
			{
				return;
			}
			PhotonNetwork.CreateRoom(roomNameInputField.text);
			MenuManager.Instance.OpenMenu("loading");
		}
		public void StartGame()
		{
			PhotonNetwork.LoadLevel(1);
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
			MenuManager.Instance.OpenMenu("loading");
		}

		public void JoinRoom(RoomInfo info)
		{
			PhotonNetwork.JoinRoom(info.Name);
			MenuManager.Instance.OpenMenu("loading");
		}
		#endregion
	}
}
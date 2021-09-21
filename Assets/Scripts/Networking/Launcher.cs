using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace Networking
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private const string GameVersion = "1";

        [SerializeField] private byte maxPlayersPerRoom = 4;
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject progressLabel;

        private bool _isConnecting;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
        }

        public void Connect()
        {
            controlPanel.SetActive(false);
            progressLabel.SetActive(true);
        
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
                return;
            }

            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
        }

        public void QuitGame()
        {
            Debug.Log("Quit game!");
            Application.Quit();
        }

        public override void OnConnectedToMaster()
        {
            if (!_isConnecting) return;
        
            PhotonNetwork.JoinRandomRoom();
            _isConnecting = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
            _isConnecting = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            PhotonNetwork.LoadLevel("Main");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel("Launcher");
        }
    }
}
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            PhotonNetwork.LoadLevel("Main");
        }
    }
}
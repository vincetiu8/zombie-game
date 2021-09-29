using System.ComponentModel;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Menus_UI;
namespace UI
{
	/// <summary>
	///     Controls the pause menu in the game
	/// </summary>
	public class PauseMenu : MonoBehaviour
	{
		private PlayerInput _playerInput;

		private void Start()
		{
			if (_playerInput == null) _playerInput = GameManager.instance.localPlayer.GetComponent<PlayerInput>();
		}

		public void QuitFromPauseMenu()
		{
			PhotonNetwork.LeaveRoom();
		}

		public void QuitGameFromPauseMenu()
		{
			Application.Quit();
		}
	}
}
using System.ComponentModel;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
	// PauseMenu controls the pause menu in the game
	public class PauseMenu : MonoBehaviour
	{
		// Method is used by input, so needs to be public
		// ReSharper disable once MemberCanBePrivate.Global
		public void PauseMenuToggle()
		{
			// Checks this here since the pause menu may be instantiated before the player is
			// The player input is located on the player
			if (_playerInput == null) _playerInput = GameManager.instance.localPlayer.GetComponent<PlayerInput>();

			_gamePaused = !_gamePaused;
			string actionMap = _gamePaused ? "UI" : "Game";
			_playerInput.SwitchCurrentActionMap(actionMap);
			pauseMenuUI.SetActive(_gamePaused);
		}

		private void PauseAction(InputAction.CallbackContext context)
		{
			if (context.performed) PauseMenuToggle();
		}

		public void QuitFromPauseMenu()
		{
			PhotonNetwork.LeaveRoom();
		}

		public void QuitGameFromPauseMenu()
		{
			Application.Quit();
		}

		#region Variables

		[Header("Pause Menu Objects")] [Description("The UI with the pause menu")] [SerializeField]
		private GameObject pauseMenuUI;

		private bool        _gamePaused;
		private PlayerInput _playerInput;

		#endregion
	}
}
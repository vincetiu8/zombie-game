using Networking;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
	/// <summary>
	///     Controls the pause menu in the game
	/// </summary>
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField] private bool enablePausing;

		private bool        _gamePaused;
		private PlayerInput _playerInput;

		public void PauseMenuToggle()
		{
			// Checks this here since the pause menu may be instantiated before the player is
			// The player input is located on the player
			if (_playerInput == null)
				_playerInput = GameManager.Instance.localPlayerInstance.GetComponent<PlayerInput>();

			_gamePaused = !_gamePaused;
			string actionMap = _gamePaused ? "UI" : "Game";
			_playerInput.SwitchCurrentActionMap(actionMap);
			gameObject.SetActive(_gamePaused);
		}

		private void PauseAction(InputAction.CallbackContext context)
		{
			if (!enablePausing) return;

			if (context.performed) PauseMenuToggle();
		}

		public void QuitFromPauseMenu()
		{
			PhotonNetwork.Disconnect();
			PhotonNetwork.LoadLevel(0);
		}

		public void QuitGameFromPauseMenu()
		{
			QuitFromPauseMenu();

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
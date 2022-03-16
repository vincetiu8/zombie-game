using Networking;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menus
{
	/// <summary>
	///     Controls the pause menu in the game
	/// </summary>
	public class PauseMenu : MenuManager
	{
		[SerializeField] private bool           enablePausing;
		[SerializeField] private MenuController pauseMenuController;

		private bool        _gamePaused;
		private PlayerInput _playerInput;

		private void Start()
		{
			if (_playerInput == null && GameManager.Instance != null)
				_playerInput = GameManager.Instance.localPlayerInstance.GetComponent<PlayerInput>();
		}

		public void PauseMenuToggle()
		{
			if (!enablePausing) return;

			Debug.Log("toggle pause menu" + !_gamePaused);

			// Checks this here since the pause menu may be instantiated before the player is
			// The player input is located on the player
			if (_playerInput == null)
				_playerInput = GameManager.Instance.localPlayerInstance.GetComponent<PlayerInput>();

			_playerInput.enabled = _gamePaused;
			_gamePaused = !_gamePaused;
			OpenMenu(_gamePaused ? pauseMenuController : null);
		}

		public void PauseAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			Debug.Log("button press");
			PauseMenuToggle();
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
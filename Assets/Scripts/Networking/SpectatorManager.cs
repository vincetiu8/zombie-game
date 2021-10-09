using UnityEngine;
using UnityEngine.InputSystem;

namespace Networking
{
	[RequireComponent(typeof(GameManager))]
	public class SpectatorManager : MonoBehaviour
	{
		private int         _currentPlayer;
		private GameManager _gameManager;
		private bool        _playerAlive;

		private void Awake()
		{
			_playerAlive = true;
			_currentPlayer = -1;
		}

		private void Start()
		{
			_gameManager = GetComponent<GameManager>();
		}

		public void OnPlayerDeath()
		{
			_playerAlive = false;
			Debug.Log("Dead!");
		}

		public void NextPlayerAction(InputAction.CallbackContext context)
		{
			if (_playerAlive || !context.performed) return;

			SetNewPlayer(1);
		}

		public void PreviousPlayerAction(InputAction.CallbackContext context)
		{
			if (_playerAlive || !context.performed) return;

			SetNewPlayer(-1);
		}

		private void SetNewPlayer(int direction)
		{
			int player = _currentPlayer + direction;
			while (player != _currentPlayer)
			{
				GameObject playerInstance = _gameManager.GetPlayerInstance(player);
				if (playerInstance != null)
				{
					playerInstance.GetComponentInChildren<Camera>().enabled = true;
					GameObject previousPlayer = _gameManager.GetPlayerInstance(_currentPlayer);
					previousPlayer.GetComponentInChildren<Camera>().enabled = false;
					return;
				}

				player += direction;
			}

			// If loop exits, no players left, everyone's dead
			Debug.Log("No player found, everyone's dead");
		}
	}
}
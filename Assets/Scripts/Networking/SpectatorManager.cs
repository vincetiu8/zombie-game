using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace Networking
{
	[RequireComponent(typeof(GameManager))]
	public class SpectatorManager : MonoBehaviour
	{
		private int         _currentPlayerIndex;
		private GameManager _gameManager;
		private bool        _playerAlive;

		private void Awake()
		{
			_playerAlive = true;
		}

		private void Start()
		{
			_gameManager = GetComponent<GameManager>();
			_currentPlayerIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();
		}

		public void OnPlayerDeath(int playerIndex)
		{
			_playerAlive = false;

			if (playerIndex == _currentPlayerIndex) SetNewPlayer(1);
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
			int numPlayers = _gameManager.playerInstances.Length;
			int previousPlayerIndex = _currentPlayerIndex;
			GameObject playerInstance;

			// Loop until we find a suitable player
			do
			{
				_currentPlayerIndex = (_currentPlayerIndex + direction + numPlayers) % numPlayers;
				playerInstance = _gameManager.playerInstances[_currentPlayerIndex];
				if (playerInstance != null) break;
			} while (previousPlayerIndex != _currentPlayerIndex);

			if (playerInstance == null)
			{
				Debug.Log("No suitable players found, everyone's dead");
				return;
			}

			if (_currentPlayerIndex == previousPlayerIndex)
			{
				Debug.Log("Only one player left, can't change");
				return;
			}

			Debug.Log($"Viewing player {_currentPlayerIndex}!");
			TogglePlayerComponents(playerInstance, true);
			GameObject previousPlayer = _gameManager.playerInstances[previousPlayerIndex];
			if (previousPlayer == null) return;
			TogglePlayerComponents(previousPlayer, false);
		}

		private static void TogglePlayerComponents(GameObject playerInstance, bool toggle)
		{
			playerInstance.GetComponentInChildren<Camera>().enabled = toggle;
			playerInstance.GetComponentInChildren<Light2D>().enabled = toggle;
		}
	}
}
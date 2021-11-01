using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Networking
{
	[RequireComponent(typeof(GameManager))]
	public class SpectatorManager : MonoBehaviour
	{
		private int         _currentPlayerIndex;
		private GameManager _gameManager;
		private PlayerInput _playerInput;

		private void Start()
		{
			_gameManager = GetComponent<GameManager>();
			_playerInput = GetComponent<PlayerInput>();
			_playerInput.enabled = false;
			_currentPlayerIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();
		}

		public void OnPlayerDeath(int playerIndex)
		{
			if (playerIndex != PhotonNetwork.LocalPlayer.GetPlayerNumber()) return;

			_playerInput.enabled = true;
			SetNewPlayer(1);
		}

		public void NextPlayerAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			SetNewPlayer(1);
		}

		public void PreviousPlayerAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			SetNewPlayer(-1);
		}

		private void SetNewPlayer(int direction)
		{
			int numPlayers = _gameManager.PlayerInstances.Count;

			if (numPlayers == 0) return;

			int minDist = int.MaxValue;
			int newPlayerNumber = _currentPlayerIndex;

			foreach (int playerNumber in _gameManager.PlayerInstances.Keys)
			{
				if (playerNumber == _currentPlayerIndex) continue;

				int dist = (playerNumber - _currentPlayerIndex * direction - 1 + numPlayers) % numPlayers;
				Debug.Log(dist + " " + playerNumber);

				if (dist >= minDist) continue;

				minDist = dist;
				newPlayerNumber = playerNumber;
			}

			Debug.Log($"Viewing player {newPlayerNumber}");

			if (_gameManager.PlayerInstances.TryGetValue(_currentPlayerIndex, out GameObject oldPlayer))
				oldPlayer.GetComponentInChildren<Camera>().enabled = false;

			_currentPlayerIndex = newPlayerNumber;
			_gameManager.PlayerInstances[_currentPlayerIndex].GetComponentInChildren<Camera>().enabled = true;
		}
	}
}
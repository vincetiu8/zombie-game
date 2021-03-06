using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shop
{
	// todo: This is all temporary and should be removed later
	// Input actions can also be deleted
	/// <summary>
	///     Allows adding and removing gold from player directly through input actions.
	///     Use this as a model to implement shops and other interactions.
	/// </summary>
	public class PlayerGoldTest : MonoBehaviour
	{
		public void AddGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
			GameManager.Instance.goldSystem.AddGold(10, playerNumber);
		}

		public void WithdrawGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			GameManager.Instance.goldSystem.WithdrawPlayerGold(10);
		}

		public void GetPlayerGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
			int gold = GameManager.Instance.goldSystem.GetPlayerGold(playerNumber);
			Debug.Log($"Player {playerNumber} has {gold} gold");
		}
	}
}
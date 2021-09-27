using Networking;
using Photon.Pun;
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

			int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
			GameManager.instance.goldSystem.AddGold(10, playerNumber);
		}

		public void WithdrawGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			GameManager.instance.goldSystem.WithdrawPlayerGold(10);
		}

		public void GetPlayerGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
			int gold = GameManager.instance.goldSystem.GetPlayerGold(playerNumber);
			Debug.Log($"Player {playerNumber} has {gold} gold");
		}
	}
}
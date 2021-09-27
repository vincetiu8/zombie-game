using System.Collections.Generic;
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
	public class PlayerGoldTest : MonoBehaviourPun
	{
		public void AddGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			photonView.RPC("RPCAddGold", RpcTarget.MasterClient, 10);
		}

		[PunRPC]
		private void RPCAddGold(int goldAmount, PhotonMessageInfo info)
		{
			GameManager.instance.goldSystem.AddGold(new List<int> { info.Sender.ActorNumber }, goldAmount);
		}

		public void WithdrawGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			photonView.RPC("RPCWithdrawGold", RpcTarget.MasterClient, 10);
		}

		[PunRPC]
		private void RPCWithdrawGold(int goldAmount, PhotonMessageInfo info)
		{
			GameManager.instance.goldSystem.WithdrawGold(info.Sender.ActorNumber, goldAmount);
		}

		public void GetPlayerGoldAction(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			photonView.RPC("RPCGetPlayerGold", RpcTarget.MasterClient);
		}

		[PunRPC]
		private void RPCGetPlayerGold(PhotonMessageInfo info)
		{
			int gold = GameManager.instance.goldSystem.GetPlayerGold(info.Sender.ActorNumber);
			Debug.Log($"Player {info.Sender.ActorNumber} has {gold} gold");
		}
	}
}
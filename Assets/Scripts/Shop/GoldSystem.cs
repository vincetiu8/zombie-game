using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Shop
{
	/// <summary>
	///     Handles accessing information and updating gold.
	/// </summary>
	public class GoldSystem : MonoBehaviourPunCallbacks
	{
		public UnityEvent playerGoldChanged;

		// Dictionary that contains all the players and the gold they have
		// Makes it easier to display all player's gold on the UI as well
		private List<int> _playerGoldAmounts;

		private void Awake()
		{
			_playerGoldAmounts = new List<int>();
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) _playerGoldAmounts.Add(0);

			playerGoldChanged = new UnityEvent();
		}

		private bool IsInvalidPlayer(int playerNumber)
		{
			return 0 > playerNumber || playerNumber >= _playerGoldAmounts.Count;
		}

		/// <summary>
		///     Adds a set amount of gold to all players.
		/// </summary>
		/// <param name="goldAmount">The amount of gold to add</param>
		public void AddGold(int goldAmount)
		{
			photonView.RPC("RPCAddGold", RpcTarget.All, goldAmount);
		}

		[PunRPC]
		private void RPCAddGold(int goldAmount)
		{
			// Loop through all players and add gold
			foreach (Player player in PhotonNetwork.PlayerList) RPCAddPlayerGold(goldAmount, player.GetPlayerNumber());
		}

		/// <summary>
		///     Adds a set amount of gold to a specified player.
		/// </summary>
		/// <param name="goldAmount">The amount of gold to add</param>
		/// <param name="playerNumber">The numbers of the player to add the gold to</param>
		public void AddGold(int goldAmount, int playerNumber)
		{
			if (IsInvalidPlayer(playerNumber))
			{
				Debug.LogError($"Trying to add gold to player {playerNumber}, out of bounds");
				return;
			}

			photonView.RPC("RPCAddPlayerGold", RpcTarget.All, goldAmount, playerNumber);
		}

		/// <summary>
		///     Adds a set amount of gold to all specified players.
		/// </summary>
		/// <param name="goldAmount">The amount of gold to add</param>
		/// <param name="playerNumbers">The numbers of the players to add the gold to</param>
		public void AddGold(int goldAmount, List<int> playerNumbers)
		{
			foreach (int playerNumber in playerNumbers)
			{
				if (IsInvalidPlayer(playerNumber))
				{
					Debug.LogError($"Trying to add gold to player {playerNumber}, out of bounds");
					return;
				}

				photonView.RPC("RPCAddPlayerGold", RpcTarget.All, goldAmount, playerNumber);
			}
		}

		[PunRPC]
		private void RPCAddPlayerGold(int goldAmount, int playerNumber)
		{
			_playerGoldAmounts[playerNumber] += goldAmount;
			playerGoldChanged.Invoke();
			Debug.Log($"Added {goldAmount} gold to player {playerNumber}");
		}

		/// <summary>
		///     Attempts to withdraw a certain amount of gold from the local player's balance.
		/// </summary>
		/// <param name="goldAmount">The amount of gold to withdraw</param>
		/// <returns>Whether the gold was withdrawn successfully</returns>
		public bool WithdrawPlayerGold(int goldAmount)
		{
			int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();

			if (IsInvalidPlayer(playerNumber))
			{
				Debug.LogError($"Trying to withdraw gold from player {playerNumber}, out of bounds");
				return false;
			}

			if (goldAmount > _playerGoldAmounts[playerNumber])
			{
				Debug.Log($"Trying to withdraw {goldAmount} gold from player {playerNumber}, not enough money");
				return false;
			}

			photonView.RPC("RPCWithdrawGold", RpcTarget.All, goldAmount);
			return true;
		}

		[PunRPC]
		public void RPCWithdrawGold(int goldAmount, PhotonMessageInfo info)
		{
			int playerNumber = info.Sender.GetPlayerNumber();
			_playerGoldAmounts[playerNumber] -= goldAmount;
			playerGoldChanged.Invoke();
			Debug.Log($"Player {playerNumber} successfully withdrew {goldAmount} gold");
		}

		/// <summary>
		///     Gets the amount of gold the local player has.
		/// </summary>
		/// <returns>The amount of gold the local player has</returns>
		public int GetPlayerGold()
		{
			return GetPlayerGold(PhotonNetwork.LocalPlayer.GetPlayerNumber());
		}

		/// <summary>
		///     Gets the amount of gold a player currently has.
		/// </summary>
		/// <param name="playerNumber">The player's number</param>
		/// <returns>The amount of gold the player has</returns>
		public int GetPlayerGold(int playerNumber)
		{
			// ReSharper disable once InvertIf // Debug will be removed later
			if (IsInvalidPlayer(playerNumber))
			{
				Debug.LogError($"Trying to get player {playerNumber}'s gold, out of bounds");
				return 0;
			}

			return _playerGoldAmounts[playerNumber];
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			if (!IsInvalidPlayer(newPlayer.GetPlayerNumber())) return;

			_playerGoldAmounts.Add(0);
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			if (otherPlayer.IsInactive) return;

			_playerGoldAmounts[otherPlayer.GetPlayerNumber()] = 0;
		}
	}
}
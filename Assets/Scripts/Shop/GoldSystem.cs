using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Shop
{
	/// <summary>
	///     Handles accessing information and updating gold.
	/// </summary>
	public class GoldSystem : MonoBehaviourPun
	{
		// Dictionary that contains all the players and the gold they have
		// Makes it easier to display all player's gold on the UI as well
		private int[] _allPlayerGold;

		private void Awake()
		{
			_allPlayerGold = new int[PhotonNetwork.PlayerList.Length];
		}

		private bool IsInvalidPlayer(int playerNumber)
		{
			return 0 > playerNumber || playerNumber >= _allPlayerGold.Length;
		}

		/// <summary>
		///     Adds a set amount of gold to all specified players.
		/// </summary>
		/// <param name="playerNumbers">The numbers of the players to add the gold to</param>
		/// <param name="goldAmount">The amount of gold to add</param>
		public void AddGold(List<int> playerNumbers, int goldAmount)
		{
			foreach (int rawPlayerNumber in playerNumbers)
			{
				// Player number indexing starts at 1, need to correct for array
				int playerNumber = rawPlayerNumber - 1;

				if (IsInvalidPlayer(playerNumber))
				{
					Debug.LogError($"Trying to add gold to player {playerNumber + 1}, out of bounds");
					continue;
				}

				_allPlayerGold[playerNumber] += goldAmount;
				Debug.Log($"Added {goldAmount} gold to player {playerNumber + 1}");
			}
		}

		/// <summary>
		///     Attempts to withdraw a certain amount of gold from a player's balance.
		/// </summary>
		/// <param name="playerNumber">The number of the player to withdraw gold from</param>
		/// <param name="goldAmount">The amount of gold to withdraw</param>
		/// <returns>Whether the gold was withdrawn successfully</returns>
		public bool WithdrawGold(int playerNumber, int goldAmount)
		{
			// Player number indexing starts at 1, need to correct for array
			playerNumber--;

			if (IsInvalidPlayer(playerNumber))
			{
				Debug.LogError($"Trying to withdraw gold from player {playerNumber + 1}, out of bounds");
				return false;
			}

			if (goldAmount > _allPlayerGold[playerNumber])
			{
				Debug.Log($"Trying to withdraw {goldAmount} gold from player {playerNumber + 1}, not enough money");
				return false;
			}

			_allPlayerGold[playerNumber] -= goldAmount;
			Debug.Log($"Player {playerNumber + 1} successfully withdrew {goldAmount} gold");
			return true;
		}

		/// <summary>
		///     Gets the amount of gold a player currently has.
		/// </summary>
		/// <param name="playerNumber">The player's number</param>
		/// <returns>The amount of gold the player has</returns>
		public int GetPlayerGold(int playerNumber)
		{
			// ReSharper disable once InvertIf // Debug will be removed later
			if (IsInvalidPlayer(playerNumber - 1))
			{
				Debug.LogError($"Trying to get player {playerNumber}'s gold, out of bounds");
				return 0;
			}

			return _allPlayerGold[playerNumber - 1];
		}
	}
}
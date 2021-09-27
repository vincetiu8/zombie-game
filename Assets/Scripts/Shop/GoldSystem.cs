using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Shop
{
	/// <summary>
	///     Handles accessing information and updating gold.
	/// </summary>
	public class GoldSystem : MonoBehaviour
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
			foreach (int playerNumber in playerNumbers)
			{
				if (IsInvalidPlayer(playerNumber))
				{
					Debug.LogError($"Trying to add gold to player {playerNumber}, out of bounds");
					continue;
				}

				_allPlayerGold[playerNumber] += playerNumber;
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
			if (IsInvalidPlayer(playerNumber))
			{
				Debug.LogError($"Trying to withdraw gold from player {playerNumber}, out of bounds");
				return false;
			}

			if (goldAmount < _allPlayerGold[playerNumber])
			{
				Debug.Log($"Trying to withdraw ${goldAmount} gold from player {playerNumber}, not enough money");
				return false;
			}

			_allPlayerGold[playerNumber] -= goldAmount;
			return true;
		}

		/// <summary>
		///     Gets the amount of gold a player currently has.
		/// </summary>
		/// <param name="playerNumber">The player's number</param>
		/// <returns>The amount of gold the player has</returns>
		public int GetPlayerGold(int playerNumber)
		{
			return _allPlayerGold[playerNumber];
		}
	}
}
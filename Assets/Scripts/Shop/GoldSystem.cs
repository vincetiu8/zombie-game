using System.Collections.Generic;
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
		private readonly Dictionary<string, int> _allPlayerGold = new Dictionary<string, int>();

		/// <summary>
		///     Adds a set amount of gold to all specified players.
		/// </summary>
		/// <param name="playerNames">The names of the players to add the gold to</param>
		/// <param name="goldAmount">The amount of gold to add</param>
		public void AddGold(List<string> playerNames, int goldAmount)
		{
			foreach (string playerName in playerNames)
			{
				if (_allPlayerGold.ContainsKey(playerName))
					_allPlayerGold[playerName] += goldAmount;
			}
		}

		/// <summary>
		///     Attempts to withdraw a certain amount of gold from a player's balance.
		/// </summary>
		/// <param name="playerName">The name of the player to withdraw gold from</param>
		/// <param name="goldAmount">The amount of gold to withdraw</param>
		/// <returns>Whether the gold was withdrawn successfully</returns>
		public bool WithdrawGold(string playerName, int goldAmount)
		{
			if (_allPlayerGold.ContainsKey(playerName))
			{
				if (goldAmount < _allPlayerGold[playerName])
				{
					_allPlayerGold[playerName] -= goldAmount;
					return true;
				}

				Debug.Log("Player doesn't have enough money");
			}

			Debug.LogWarning("Attempting to withdraw gold from non-existent player");
			return false;
		}

		public int GetPlayerGold(string playerName)
		{
			return _allPlayerGold[playerName];
		}

		public Dictionary<string, int> GetAllGold()
		{
			return _allPlayerGold;
		}

		public void InitializePlayer(string playerName)
		{
			_allPlayerGold.Add(playerName, 0);
		}

		public void RemovePlayer(string playerName)
		{
			_allPlayerGold.Remove(playerName);
		}
	}
}
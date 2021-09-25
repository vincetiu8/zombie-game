using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
	// GoldSystem handles gold across players
	public class GoldSystem : MonoBehaviour
	{
		#region Variables

		// Dictionary that contains all the players and the gold they have
		// Makes it easier to display all player's gold on the UI as well
		private readonly Dictionary<string, int> _allPlayerGold = new Dictionary<string, int>();

		#endregion

		#region Methods

		// Adds a set amount of gold to all specified players
		public void AddGold(List<string> playerNames, int goldAmount)
		{
			foreach (string playerName in playerNames)
				if (_allPlayerGold.ContainsKey(playerName))
					_allPlayerGold[playerName] += goldAmount;
		}

		// Attempts to withdraw a certain amount of gold from a player's balance
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

		#endregion
	}
}
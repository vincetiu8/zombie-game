using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class GoldSystem : MonoBehaviour
    {

        private readonly Dictionary<string, int> _allPlayerGold = new Dictionary<string, int>(); 
        // Dictionary that contains all the players and the gold they have
        // Makes it easier to display all player's gold on the UI as well
        // Note for myself (debug _allPlayerGold with): foreach (KeyValuePair<string, int> kvp in goldSystem.AllPlayerGold) Debug.Log("Name: " + kvp.Key + " Gold: "+ kvp.Value);

        public void AddGold(List<string> playerNames, int goldAmount) // Takes in the names of multiple players as well
            // used like this btw: AddGold(new List<string> { PhotonNetwork.NickName }, 10);
        {
            foreach (string playerName in playerNames)
            {
                if (_allPlayerGold.ContainsKey(playerName)) _allPlayerGold[playerName] += goldAmount;
            }
        }

        public bool WithdrawGold(string playerName, int goldAmount)
            // Only coded for one person only since taking money from multiple people at the same time doesn't sound like something we need
        {
            if (_allPlayerGold.ContainsKey(playerName))
            {
                if (goldAmount < _allPlayerGold[playerName])
                {
                    _allPlayerGold[playerName] -= goldAmount;
                    return true;
                }
                else
                {
                    Debug.Log("Player doesn't have enough money");
                    return false;
                }
            }
            else
            {
                Debug.Log("Player doesn't exist");
                return false;
            }
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
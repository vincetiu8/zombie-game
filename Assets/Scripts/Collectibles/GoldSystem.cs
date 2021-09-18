using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public class GoldSystem : MonoBehaviour
    {

        public Dictionary<string, int> AllPlayerGold = new Dictionary<string, int>(); // Dictionary that contains all the players and the gold they have
        // Makes it easier to display all player's gold on the UI as well

        public void AddGold(List<string> playerNames, int goldAmount) // Takes in the names of multiple players as well
            // used like this btw: AddGold(new List<string> { PhotonNetwork.NickName }, 10);
        {
            foreach (string name in playerNames)
            {
                if (AllPlayerGold.ContainsKey(name)) AllPlayerGold[name] += goldAmount;
            }
        }

        public bool WithdrawGold(string playerName, int goldAmount)
            // Only coded for one person only since taking money from multiple people at the same time doens't sound like something we need
        {
            if (AllPlayerGold.ContainsKey(playerName))
            {
                if (goldAmount < AllPlayerGold[playerName])
                {
                    AllPlayerGold[playerName] -= goldAmount;
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

        public int GetGold(string playerName)
        {
            return AllPlayerGold[playerName];
        }
    }
}
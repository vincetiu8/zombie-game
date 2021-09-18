using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoldSystem
{
    public struct PlayerGold
    {
        public string playerName;
        public int gold;
        public PlayerGold(string nickName, int gold) : this()
        {
            this.playerName = nickName;
            this.gold = gold;
        }
    }

  

    public Dictionary<string, int> allPlayerGold = new Dictionary<string, int>();

    public void AddGold(List<string> playerNames, int goldAmount) // Takes in the names of multiple players as well
    {
        foreach (string name in playerNames)
        {
            if (allPlayerGold.ContainsKey(name))
            {
                allPlayerGold[name] += goldAmount;
            }
        }
    }

    public void WithdrawGold(List<string> playerNames, int goldAmount)
    {
        foreach (string name in playerNames)
        {
            if (allPlayerGold.ContainsKey(name))
            {
                if (goldAmount < allPlayerGold[name])
                {
                    allPlayerGold[name] -= goldAmount;
                }
               
            }
        }
    }
}
public class Gold : MonoBehaviour
{
    private GoldSystem.PlayerGold playerGold = new GoldSystem.PlayerGold(PhotonNetwork.NickName, 0);

    

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public static class GoldSystem
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
    public static void AddGold(string playerName, int goldAmount)
    {
        if (playerName != PlayerGold.playerName) return // how am i supposed to get the player name from the player?
        playerGold.gold += goldAmount;
    }
}
public class Gold : MonoBehaviour
{
    private GoldSystem.PlayerGold playerGold = new GoldSystem.PlayerGold(PhotonNetwork.NickName, 0);

    

}


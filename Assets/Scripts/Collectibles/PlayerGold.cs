using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Collectibles
{
    public class PlayerGold : GoldSystem
    {
        private void Awake()
        {
            AllPlayerGold.Add(PhotonNetwork.NickName, 0);

            foreach (KeyValuePair<string, int> kvp in AllPlayerGold)
                Debug.Log("Name: " + kvp.Key + " Gold: "+ kvp.Value);

        }

    }
}
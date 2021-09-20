using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Collectibles
{
    public class PlayerGold : MonoBehaviour
    {
        public GoldSystem goldSystem;
        
        private void Start()
        {
            Debug.Log(goldSystem);
            goldSystem.AllPlayerGold.Add(PhotonNetwork.NickName, 0);

            foreach (KeyValuePair<string, int> kvp in goldSystem.AllPlayerGold)
                Debug.Log("Name: " + kvp.Key + " Gold: "+ kvp.Value);

        }

    }
}
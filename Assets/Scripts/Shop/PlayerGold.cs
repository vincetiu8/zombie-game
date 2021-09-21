using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Shop
{
    public class PlayerGold : MonoBehaviour
    {
        public GoldSystem goldSystem;
        
        private void Start()
        {
            goldSystem.AllPlayerGold.Add(PhotonNetwork.NickName, 0);

            foreach (KeyValuePair<string, int> kvp in goldSystem.AllPlayerGold)
                Debug.Log("Name: " + kvp.Key + " Gold: "+ kvp.Value);
            
            // Check gold amount for all the players

        }

    }
}
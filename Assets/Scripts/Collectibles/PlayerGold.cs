using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGold : GoldSystem
{
    private void Awake()
    {
        allPlayerGold.Add(PhotonNetwork.NickName, 0);

        foreach (KeyValuePair<string, int> kvp in allPlayerGold)
            Debug.Log("Name: " + kvp.Key + " Gold: "+ kvp.Value);


    }

    private void Update()
    {
        
    }

}
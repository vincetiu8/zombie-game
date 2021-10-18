using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("Necromancer", transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

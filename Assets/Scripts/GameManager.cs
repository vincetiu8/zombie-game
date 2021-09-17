using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject playerPrefab;

    private int spawn;

    private void Start() {
        spawn = Random.Range(1, 4);
        Debug.Log(spawn);
        if (spawn == 1) {
            PhotonNetwork.Instantiate(playerPrefab.name, Vector2.zero, Quaternion.identity);
        }
        else if (spawn == 2) {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(6, -11), Quaternion.identity);
        }
        else if(spawn==3) {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-10, 7), Quaternion.identity);
        }
    }
}
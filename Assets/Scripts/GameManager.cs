using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public GameObject player;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnpoints;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    private void Start()
    {
        // Selecting spawn randomly from a range
        // todo: once rooms are implemented, have the spawn predetermined by the player's room number
        Debug.Log("Spawning Player!");
        int position = Random.Range(0, spawnpoints.Length);
        Vector3 spawnPosition = spawnpoints[position].position;
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
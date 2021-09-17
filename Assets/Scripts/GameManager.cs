using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnpoints;

    private void Start()
    {
        // Selecting spawn randomly from a range
        // todo: once rooms are implemented, have the spawn predetermined by the player's room number
        Debug.Log("Spawning Player!");
        int position = Random.Range(0, spawnpoints.Length);
        Vector3 spawnPosition = spawnpoints[position].position;
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
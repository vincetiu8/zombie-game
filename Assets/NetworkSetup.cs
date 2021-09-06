using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] behavioursToDisable;

    private void Start()
    {
        if (isLocalPlayer) return;
        
        foreach (Behaviour behaviour in behavioursToDisable)
        {
            behaviour.enabled = false;
        }
    }
}

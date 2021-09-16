using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Lantern : MonoBehaviour {
    [SerializeField] Behaviour[] lightswitch;
    [SerializeField] float duration=30f;
    private bool on;
    private void Start() {
        lightswitch[0].enabled = false;
        StartCoroutine(light());
    }

    private void Update() {;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            Debug.Log(on);
            lightswitch[0].enabled = true;
            on = true;
        }                      
    }
    
    IEnumerator light(){
        while (true) {
            yield return new WaitForSeconds(duration);
            lightswitch[0].enabled = false;
            on = false;
        }
    }
}

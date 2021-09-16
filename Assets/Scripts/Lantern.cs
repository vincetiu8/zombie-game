using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Debug = UnityEngine.Debug;

public class Lantern : MonoBehaviour {
    [SerializeField] private Behaviour[] lightswitch;
    [SerializeField] private float duration;

    private Coroutine currentCoroutine;

    private void Start() {
        for (int i = 0; i < lightswitch.Length; i++) {
            lightswitch[i].enabled = false;   
        }
    }

    private void Update() {
        Debug.Log(currentCoroutine);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine (light());
            lightswitch[0].enabled = true;
        }                      
    }
    
    IEnumerator light(){
        yield return new WaitForSeconds(duration/4);
        lightswitch[0].enabled = false;
        lightswitch[1].enabled = true;
        yield return new WaitForSeconds(duration / 4);
        lightswitch[1].enabled = false;
        lightswitch[2].enabled = true;
        yield return new WaitForSeconds(duration / 4);
        lightswitch[2].enabled = false;
        lightswitch[3].enabled = true;
        yield return new WaitForSeconds(duration / 4);
        lightswitch[3].enabled = false;
        lightswitch[0].enabled = false;
        
    }
}

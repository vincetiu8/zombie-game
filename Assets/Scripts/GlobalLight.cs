using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLight : MonoBehaviour {
    [SerializeField] Behaviour light;
    private void Awake() {
        light.enabled = false;
    }
}

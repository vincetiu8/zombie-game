using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EditorLightDisabler : MonoBehaviour {

    private Light2D _light2D;

    private void Start() {
        _light2D = GetComponent<Light2D>();
        _light2D.enabled = false;
    }
}

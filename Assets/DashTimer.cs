using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashTimer : MonoBehaviour {
	private TextMeshPro _textMeshPro;

	private void Start() {
		_textMeshPro = GetComponent<TextMeshPro>();
		_textMeshPro.color=Color.yellow;
	}
}

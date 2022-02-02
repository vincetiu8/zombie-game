using System;
using TMPro;
using UnityEngine;

namespace PlayerScripts {
	public class DashMoveTimer : MonoBehaviour {
		[SerializeField] private GameObject player;

		private PlayerMovement _playerMovement;
		private TextMeshPro    _textMeshPro;

		private void Start() {
			_playerMovement = player.GetComponent<PlayerMovement>();
			_textMeshPro = gameObject.GetComponent<TextMeshPro>();
		}

		private void Update() {
			Debug.Log(Math.Round(_playerMovement.dashTime));
			_textMeshPro.SetText(Math.Round(_playerMovement.dashTime).ToString());
		}
	}
}

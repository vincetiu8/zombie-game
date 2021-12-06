using System;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float       acceleration;
		[SerializeField] private float       startDashTime;
		[SerializeField] private float       dashSpeed;

		private Vector2 _movementDirection;

		private Rigidbody2D _rigidbody2D;

		private PlayerHealth _playerHealth;

		private float       dashTime;


		private void Awake() {
			_playerHealth = GetComponent<PlayerHealth>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			dashTime = startDashTime;
		}

		private void FixedUpdate()
		{
			_rigidbody2D.AddForce(_movementDirection * acceleration);
			if (dashTime > 0) {
				dashTime -= Time.deltaTime;
			}
		}

		public void UpdateMovementDirection(InputAction.CallbackContext context) {
			_movementDirection = context.ReadValue<Vector2>();

			if (_movementDirection.magnitude > 0.01f) {
				_playerHealth.ResetNaturalHealing();
			}
		}

		public void Dash(InputAction.CallbackContext context) {
			if (context.canceled) {
				return;
			}

			if (dashTime <= 0) {
				_rigidbody2D.AddForce(_movementDirection * dashSpeed);
				dashTime = startDashTime;
			}
		}
	}
}
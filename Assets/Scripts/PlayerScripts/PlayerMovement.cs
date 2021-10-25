using System;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float acceleration;

		private Vector2 _movementDirection;

		private Rigidbody2D _rigidbody2D;

		private PlayerHealth _playerHealth;

		private void Awake() 
		{
			_playerHealth = GetComponent<PlayerHealth>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			_rigidbody2D.AddForce(_movementDirection * acceleration);
		}

		public void UpdateMovementDirection(InputAction.CallbackContext context) {
			_movementDirection = context.ReadValue<Vector2>();

			if (_movementDirection.magnitude > 0.01f) {
				_playerHealth.ResetNaturalHealing();
			}
		}
	}
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
	public class PlayerMovement : MonoBehaviour
	{
        [SerializeField] private float acceleration;
        
        private Vector2 _movementDirection;

		private Rigidbody2D _rigidbody2D;

		private AnimatedHealth _animatedHealth;

		private Vector3 _previousPosition;

		private void Awake() 
		{
			_animatedHealth = GetComponent<AnimatedHealth>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_previousPosition = gameObject.transform.position;
		}

		private void FixedUpdate()
		{
            _rigidbody2D.AddForce(_movementDirection * acceleration);
		}

		public void UpdateMovementDirection(InputAction.CallbackContext context) {
			_animatedHealth.canHeal = context.canceled;
			_movementDirection = context.ReadValue<Vector2>();
		}
	}
}
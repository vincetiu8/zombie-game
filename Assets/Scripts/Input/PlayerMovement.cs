using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float movementSpeed;

		private Vector2 _movementDirection;

		private Rigidbody2D _rigidbody2D;

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			_rigidbody2D.velocity = _movementDirection * movementSpeed;
		}

		public void UpdateMovementDirection(InputAction.CallbackContext context)
		{
			_movementDirection = context.ReadValue<Vector2>();
		}
	}
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PlayerScripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float acceleration;

		private Vector2 _movementDirection;

		private PlayerHealth _playerHealth;

		private Rigidbody2D _rigidbody2D;

		private float dashTime;

		[SerializeField] private float      startDashTime;
		[SerializeField] private float      dashSpeed;
		[SerializeField] private GameObject _textDisplay;

		private void Awake()
		{
			_playerHealth = GetComponent<PlayerHealth>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			dashTime = startDashTime;
		}

		private void FixedUpdate()
		{
			_rigidbody2D.AddForce(_movementDirection * acceleration);
			if (dashTime > 0) {
				dashTime -= Time.deltaTime;
				_textDisplay.GetComponent<Text>().text = Mathf.Round(dashTime).ToString();
			}
		}

		public void UpdateMovementDirection(InputAction.CallbackContext context)
		{
			_movementDirection = context.ReadValue<Vector2>();

			_playerHealth.ToggleNaturalHealing(_movementDirection.magnitude <= 0.01f);
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
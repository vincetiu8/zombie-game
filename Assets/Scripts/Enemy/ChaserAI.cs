using System.ComponentModel;
using UnityEngine;

namespace Enemy
{
	// ChaserAI implements a simple script that makes the enemy move directly towards the player it's targeting
	public class ChaserAI : MonoBehaviour
	{
		#region Variables

		[Header("Chasing Settings")] [Description("The speed the enemy moves at")] [SerializeField] [Range(0.25f, 10)]
		private float movementSpeed;

		private Rigidbody2D    _rigidbody2D;
		private PlayerDetector _playerDetector;

		#endregion

		#region Methods

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerDetector = GetComponentInChildren<PlayerDetector>();
			if (_playerDetector == null)
				Debug.LogError("No player detector present in children, can't track any players.");
		}

		private void FixedUpdate()
		{
			// Set the velocity in the direction of the player
			Vector2 direction = _playerDetector.GetTrackingPlayerDirection();
			_rigidbody2D.velocity = direction * movementSpeed;

			// Rotate the enemy towards the player
			float angle = Utils.Vector2ToDeg(direction);
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		#endregion
	}
}
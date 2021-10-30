using System.ComponentModel;
using UnityEngine;
using Utils;

namespace Enemy
{
	/// <summary>
	///     Makes the enemy move directly towards the player it's targeting.
	/// </summary>
	public abstract class ChaserAI : MonoBehaviour
	{
		private static readonly int MovementSpeedProperty = Animator.StringToHash("Move Speed");

		[Header("Chasing Settings")] [SerializeField] [Range(0.1f, 5f)]
		private float acceleration = 1;

        private float _baseAcceleration;

		[SerializeField] [Description("How steeply enemy offsets the force when taking a corner")] [Range(-10f, 10f)]
		private float cornerTakingAngleMultiplier = 1;

		[SerializeField] [Range(0.01f, 0.5f)] private float turningSmoothing = 0.1f;

		private bool _canMove = true;

		private   Rigidbody2D _rigidbody2D;
		protected Vector2     Destination;

		protected Transform TrackingPlayer;

		protected virtual void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		protected virtual void Start()
        {
            _baseAcceleration = acceleration;
			Animator animator = GetComponentInChildren<Animator>();
			animator.SetFloat(MovementSpeedProperty, acceleration / 2);
		}

		protected virtual void FixedUpdate()
		{
			if (!TrackingPlayer) return;

			// Set the velocity in the direction of the destination
			SetRigidbodyAttributes();
		}

		public virtual void SetPlayerToTrack(Transform player)
		{
			TrackingPlayer = player;
			if (!TrackingPlayer) _rigidbody2D.velocity = Vector2.zero;
		}

		protected Vector2 GetMovementDirection()
		{
			return Destination - (Vector2)transform.position;
		}

		private void SetRigidbodyAttributes()
		{
			SetRigidbodyAttributes(GetMovementDirection());
		}

		protected void SetRigidbodyAttributes(Vector2 movementDirection)
		{
			if (!_canMove) return;
			float angle = TransformUtils.Vector2ToDeg(movementDirection);

			// Smooth the angle - prevent the enemy from turning too fast
			angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.deltaTime / turningSmoothing);

			// Rotate the enemy towards the destination
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			float accountedAngle = (TransformUtils.Vector2ToDeg(transform.InverseTransformPoint(Destination)));
			_rigidbody2D
				.AddForce(TransformUtils.DegToVector2(angle + accountedAngle * cornerTakingAngleMultiplier) * acceleration,
				          (ForceMode2D)ForceMode.Force);
		}

		public void DisableMovement(bool disable)
		{
			_canMove = !disable;
		}

        public void ScaleAcceleration(float scale)
        {
            acceleration *= scale;
        }

        public void ResetAcceleration()
        {
            acceleration = _baseAcceleration;
        }

        public Transform GetTrackingPlayer()
        {
            return TrackingPlayer;
        }
	}
}
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

		[Header("Chasing Settings")] [SerializeField] [Range(0.1f, 2.5f)]
		private float movementSpeed = 1;
        
        [SerializeField] [Range(0.1f, 2.5f)]
        private float maxSpeed = 1;
        
        [SerializeField] [Range(0.1f, 2.5f)]
        private float acceleration = 1;
        

		[SerializeField] [Range(0.01f, 0.5f)] private float turningSmoothing = 0.1f;

		private   Rigidbody2D _rigidbody2D;
		protected Vector2     Destination;

		protected Transform TrackingPlayer;

        private float _previousAngle;
        [SerializeField] private bool _startingBoost = true;

		protected virtual void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		protected virtual void Start()
		{
			Animator animator = GetComponentInChildren<Animator>();
			animator.SetFloat(MovementSpeedProperty, movementSpeed);
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
			float angle = TransformUtils.Vector2ToDeg(movementDirection);

			// Smooth the angle - prevent the enemy from turning too fast
			angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.deltaTime / turningSmoothing);

			// Rotate the enemy towards the destination
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (_startingBoost)
            {
                _rigidbody2D.AddForce(TransformUtils.DegToVector2(angle) * acceleration, (ForceMode2D) ForceMode.Force);
            }
            
            _rigidbody2D.AddForce(TransformUtils.DegToVector2(_previousAngle + 180) * acceleration, (ForceMode2D) ForceMode.Force);
            
            //_rigidbody2D.velocity = TransformUtils.DegToVector2(angle) * movementSpeed;
            
            //_rigidbody2D.velocity = Vector2.zero;
            
            //float accountedAngle = Vector3.Angle(_rigidbody2D.velocity, (Vector3)Destination - transform.position);
            _rigidbody2D.AddForce(TransformUtils.DegToVector2(angle) * acceleration, (ForceMode2D) ForceMode.Force);

            //if (_rigidbody2D.velocity.magnitude >= maxSpeed) _rigidbody2D.velocity = maxSpeed*TransformUtils.DegToVector2(angle);

            _previousAngle = angle;
        }
	}
}
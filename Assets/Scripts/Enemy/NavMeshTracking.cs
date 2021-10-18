using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
	/// <summary>
	///     Move enemy to position through the base NavMeshConfiguration
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class NavMeshTracking : ChaserAI
	{
		[Header("Navmesh Settings")] [SerializeField] [Range(0.1f, 2.5f)]
		private float updateCooldown = 0.5f;

		[Description("The minimum distance to the current path corner before skipping to the next corner")]
		[SerializeField]
		[Range(0.1f, 2.5f)]
		private float minimumProximity = 0.5f;

		private NavMeshAgent _navMeshAgent;
		private NavMeshPath  _path;
		private int          _pathCorner;
		private float        _updateCooldown;

		protected override void Awake()
		{
			base.Awake();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_path = new NavMeshPath();
		}

		protected override void Start()
		{
			base.Start();
			_navMeshAgent.isStopped = true;
			_navMeshAgent.updateUpAxis = false;
		}

		private void Update()
		{
			if (_updateCooldown > 0)
			{
				_updateCooldown -= Time.deltaTime;
				return;
			}

			if (TrackingPlayer == null) return;

			UpdatePath();
		}

		protected override void FixedUpdate()
		{
			Vector2 movementDirection = GetMovementDirection();

			if (movementDirection.magnitude < minimumProximity && _pathCorner < _path.corners.Length - 1)
			{
				_pathCorner++;
				Destination = _path.corners[_pathCorner];
			}

			SetRigidbodyAttributes(movementDirection);
		}

		public override void SetPlayerToTrack(Transform player)
		{
			base.SetPlayerToTrack(player);

			UpdatePath();
		}

		private void UpdatePath()
		{
			if (TrackingPlayer == null) return;

			bool success = _navMeshAgent.CalculatePath(TrackingPlayer.position, _path);

			if (!success)
			{
				Debug.Log("No path found to target");
				Destination = transform.position;
				return;
			}

			Destination = _path.corners[1];
            Debug.DrawLine(transform.position,Destination);
			_pathCorner = 1;
			_updateCooldown = updateCooldown;
		}
	}
}
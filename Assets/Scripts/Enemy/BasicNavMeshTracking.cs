using System;
using UnityEngine;
using UnityEngine.AI;
namespace Enemy
{
    /// <summary>
    /// Move enemy to position through the base NavMeshConfiguration
    /// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
    public class BasicNavMeshTracking : ChaserAI
    {
        private bool _isMoving;
		private NavMeshAgent _agent;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_agent.updateUpAxis = false;
		}

		public override void MoveTowardsPlayer(Transform player)
		{
			// Stop walking if player no longer tracked
			if (player == null)
            {
                _isMoving = false;
				_agent.SetDestination(transform.position);
				return;
			}

            _isMoving = true;
			// Sets a position object will walk towards using NavMesh navigation system.
			_agent.SetDestination(player.position);
        }

        private void FixedUpdate()
        {
            if (!_isMoving) return;
            // Make agent look the direction it is going
            Vector3 dir = _agent.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

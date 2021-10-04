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
		private NavMeshAgent _agent;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_agent.updateUpAxis = false;
		}

        public override void SetPlayerToTrack(Transform player)
        {
            base.SetPlayerToTrack(player);
            if (!_playerTracked)
            {
                _agent.SetDestination(transform.position);
            }
            
        }


        
        private void FixedUpdate()
        {
            if (!_playerTracked) return;

            _agent.SetDestination(_playerTracked.position);

            // Make agent look the direction it is going
            transform.rotation = Quaternion.AngleAxis(Utils.Vector2ToDeg(_agent.velocity), Vector3.forward);
        }
    }
}

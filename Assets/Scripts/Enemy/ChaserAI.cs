using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
	/// <summary>
	///     Makes the enemy move directly towards the player it's targeting.
	/// </summary>
	
    public class ChaserAI : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private PlayerDetector _playerDetector;

        private void Awake()
        {
            _playerDetector = GetComponentInChildren<PlayerDetector>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateUpAxis = false;
        }

        private void FixedUpdate()
        {
            if (_playerDetector.GetTrackingPlayerDirection() == Vector2.zero) Debug.Log("No player in sight");
                _agent.SetDestination(_playerDetector.GetTrackingPlayerDirection());

            // Make agent look the direction it is going
            Vector3 dir = _agent.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        }
    }
}
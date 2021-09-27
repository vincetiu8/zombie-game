using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	///     Makes the enemy move directly towards the player it's targeting.
	/// </summary>

	public class ChaserAI : MonoBehaviour
	{
		private PlayerDetector _playerDetector;

		//private BasicNaveMeshTracking _trackResponse;
		private ITrackingMethod _trackMethod;

		private void Awake()
		{
			_trackMethod = GetComponent<ITrackingMethod>();
			_playerDetector = GetComponentInChildren<PlayerDetector>();
		}

		private void FixedUpdate()
		{
	
				_trackMethod.MoveTowardsPlayer(_playerDetector.GetTrackingPlayer());
			
		}


	}
}

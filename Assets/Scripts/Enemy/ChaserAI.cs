using System.Runtime.CompilerServices;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	///     Makes the enemy move directly towards the player it's targeting.
	/// </summary>

	public abstract class ChaserAI : MonoBehaviour
	{
		private PlayerDetector  _playerDetector;

		protected virtual void Awake()
		{
			_playerDetector = GetComponentInChildren<PlayerDetector>();
		}

		private void FixedUpdate()
		{
			MoveTowardsPlayer(_playerDetector.GetTrackingPlayer());
		}

		protected virtual void MoveTowardsPlayer(Transform player)
		{
		}

	}
}

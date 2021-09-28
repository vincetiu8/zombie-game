using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	///     Tracks a player based on proximity.
	///     The stickiness can be set to make the enemy prefer continuing to track a player instead of switching.
	/// </summary>
	public class PlayerDetector : MonoBehaviour
	{
		[Header("Tracker Configuration")]
		[Description("How much the enemy 'sticks' to the current player it's tracking")]
		[Range(1, 5f)]
		[SerializeField]
		private float targetStickiness = 1;

		[Description("How often the enemy updates the player it's tracking, aside from trigger events")]
		[Range(0.1f, 5f)]
		[SerializeField]
		private float updatePeriod = 1;

		private Transform _trackingPlayer;
		private float     _updateCooldown;
		private List<Transform> _players;

        private ChaserAI  _chaserAI;

        protected virtual void Awake()
        {
            _players = new List<Transform>();
            _chaserAI = GetComponentInParent<ChaserAI>();

        }

		private void Update()
		{
            if (_trackingPlayer)
            {
                _chaserAI.MoveTowardsPlayer(_trackingPlayer);
            }
            
			if (_updateCooldown > 0)
			{
				_updateCooldown -= Time.deltaTime;
				return;
			}
            UpdateTrackingPlayer();
		}

		
		/// <summary>
		///     Updates the tracking player.
		/// </summary>
		private void UpdateTrackingPlayer()
		{
			float minDistance = float.PositiveInfinity;

			foreach (Transform player in _players)
			{
				float playerDistance = (player.position - transform.position).magnitude;

				// We give priority to the current player we're tracking based on "stickiness"
				// This prevents the enemy from being stuck deciding between players
				if (playerDistance * targetStickiness > minDistance) continue;

				minDistance = playerDistance;
				_trackingPlayer = player;
			}
            _updateCooldown = updatePeriod;

        }

		/// <summary>
		///     Adds a player to the list of tracked players.
		/// </summary>
		protected void AddPlayer(Transform other)
		{
			_players.Add(other);

			// If no players are currently getting tracked, _updateCooldown can be ignored.
			if (GetTrackingPlayer() != null && _updateCooldown > 0) return;
			UpdateTrackingPlayer();
		}

		/// <summary>
		///     Removes a player from the list of tracked players
		/// </summary>
		protected void RemovePlayer(Transform other)
		{
			_players.Remove(other);
			if (_trackingPlayer != other) return;

			if (_players.Count == 0)
			{
				_trackingPlayer = null;
				return;
			}

			UpdateTrackingPlayer();
		}

		/// <summary>
		///     Returns player currently getting tracked
		/// </summary>
        public Transform GetTrackingPlayer()
        {
            if (!_trackingPlayer)
            {
	            Debug.Log("No player in sight");
                return null;
            }
            return _trackingPlayer;
        }
        
        
    }
}
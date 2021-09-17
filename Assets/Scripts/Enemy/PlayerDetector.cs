using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Enemy
{
    // This script can be expanded in the future if other tracking methods are needed
    // Make into an abstract class and make child classes for each tracking strategy
    public class PlayerDetector : MonoBehaviour
    {
        [Description("How much the enemy 'sticks' to the current player it's tracking")]
        [Range(1, 5f)] [SerializeField] private float targetStickiness = 1;
        
        [Description("How often the enemy updates the player it's tracking, aside from trigger events")]
        [Range(0.1f, 5f)] [SerializeField] private float updatePeriod = 1;
        
        private Transform _trackingPlayer;
        private List<Transform> _players;
        private float _updateCooldown;

        private void Awake()
        {
            _players = new List<Transform>();
        }

        private void Update()
        {
            if (_updateCooldown > 0)
            {
                _updateCooldown -= Time.deltaTime;
                return;
            }
            
            UpdateTrackingPlayer();
        }

        private void UpdateTrackingPlayer()
        {
            if (_updateCooldown > 0) return;

            float minDistance = float.PositiveInfinity;

            foreach (Transform player in _players)
            {
                float playerDistance = (player.position - transform.position).magnitude;

                if (playerDistance * targetStickiness > minDistance) continue;
                
                minDistance = playerDistance;
                _trackingPlayer = player;
            }

            _updateCooldown = updatePeriod;
        }
        
        private void AddPlayer(Transform other)
        {
            _players.Add(other);
            
            UpdateTrackingPlayer();
        }

        private void RemovePlayer(Transform other)
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

        public Vector2 GetTrackingPlayerDirection()
        {
            if (!_trackingPlayer)
            {
                return Vector2.zero;
            }
            
            return (_trackingPlayer.position - transform.position).normalized;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            AddPlayer(other.transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            RemovePlayer(other.transform);
        }
    }
}
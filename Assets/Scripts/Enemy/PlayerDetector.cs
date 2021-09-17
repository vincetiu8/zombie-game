using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    // This script can be expanded in the future if other tracking methods are needed
    // Make into an abstract class and make child classes for each tracking strategy
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private float updatePeriod;
        
        private Transform _trackingPlayer;
        private List<Transform> _players;
        private float _updateCooldown;

        private void Awake()
        {
            _players = new List<Transform>();
        }

        private void Update()
        {
            _updateCooldown -= Time.deltaTime;
            
            if (_updateCooldown > 0) return;
            
            UpdateTrackingPlayer();
        }

        private void AddPlayer(Transform other)
        {
            _players.Add(other.transform);
            
            UpdateTrackingPlayer();
        }

        private void UpdateTrackingPlayer()
        {
            float minDistance = float.PositiveInfinity;

            foreach (Transform player in _players)
            {
                float playerDistance = (player.position - transform.position).magnitude;

                if (playerDistance > minDistance) continue;
                
                minDistance = playerDistance;
                _trackingPlayer = player;
            }

            _updateCooldown = updatePeriod;
        }

        private void RemovePlayer(Transform other)
        {
            _players.Remove(other);
            if (_trackingPlayer != other) return;

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
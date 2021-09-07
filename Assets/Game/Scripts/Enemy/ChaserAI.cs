using UnityEngine;

namespace Game.Scripts.Enemy
{
    public class ChaserAI : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;

        private Rigidbody2D _rigidbody2D;
        private PlayerDetector _playerDetector;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerDetector = GetComponentInChildren<PlayerDetector>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = _playerDetector.GetTrackingPlayerDirection() * movementSpeed;
        }
    }
}
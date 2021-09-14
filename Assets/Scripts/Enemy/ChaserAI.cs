using UnityEngine;

namespace Enemy
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
            Vector2 direction = _playerDetector.GetTrackingPlayerDirection();
            _rigidbody2D.velocity = direction * movementSpeed;
            transform.rotation = Quaternion.Euler(0, 0, Utils.Vector2ToDeg(direction));
        }
    }
}
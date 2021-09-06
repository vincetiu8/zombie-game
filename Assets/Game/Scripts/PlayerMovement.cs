using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        
        private Rigidbody2D _rigidbody2D;
        
        private Vector2 _movementDirection;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rigidbody2D.velocity = _movementDirection * movementSpeed;
        }

        public void UpdateMovementDirection(InputAction.CallbackContext context)
        {
            Debug.Log(_movementDirection);
            _movementDirection = context.ReadValue<Vector2>();
        }
    }
}
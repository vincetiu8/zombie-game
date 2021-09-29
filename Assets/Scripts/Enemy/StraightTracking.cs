using System;
using UnityEngine;
using System.ComponentModel;

namespace Enemy
{
	/// <summary>
	/// Makes the enemy move directly towards the player it's targeting.
	/// </summary>
	public class StraightTracking : ChaserAI
	{
		[Header("Chasing Settings")] [Description("The speed the enemy moves at")] [SerializeField] [Range(0.25f, 10)]
		private float movementSpeed;
		private Rigidbody2D _rigidbody2D;

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}
      
        private void FixedUpdate()
        {
            if (!_playerTracked)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }

            // Set the velocity in the direction of the player
            Vector2 direction = (_playerTracked.position - transform.position).normalized;
            _rigidbody2D.velocity = direction * movementSpeed;
            
            // Rotate the enemy towards the player
            float angle = Utils.Vector2ToDeg(direction);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    
	}
}

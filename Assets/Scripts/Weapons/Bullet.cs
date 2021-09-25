using UnityEngine;

namespace Weapons
{
	// Bullet is a one-time collision damager with a lifetime
	public class Bullet : CollisionDamager
	{
		#region Variables

		public float lifetime;

		private float _timeAlive;

		#endregion

		#region Methods

		protected override void Update()
		{
			_timeAlive += Time.deltaTime;

			// Destroy the bullet if its outlived its lifetime
			if (_timeAlive > lifetime) Destroy(gameObject);

			base.Update();
		}

		// Destroy the bullet on a collision
		protected override void OnCollisionStay2D(Collision2D other)
		{
			base.OnCollisionStay2D(other);

			Destroy(gameObject);
		}

		#endregion
	}
}
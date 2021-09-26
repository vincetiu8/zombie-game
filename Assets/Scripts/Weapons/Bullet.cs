using UnityEngine;

namespace Weapons
{
	/// <summary>
	///     A one-time collision damager with a lifetime.
	/// </summary>
	public class Bullet : CollisionDamager
	{
		public float lifetime;

		private float _timeAlive;

		protected override void Update()
		{
			_timeAlive += Time.deltaTime;

			// Destroy the bullet if its outlived its lifetime
			if (_timeAlive > lifetime) Destroy(gameObject);

			base.Update();
		}

		protected override void OnCollisionStay2D(Collision2D other)
		{
			base.OnCollisionStay2D(other);

			// Destroy the bullet on a collision
			Destroy(gameObject);
		}
	}
}
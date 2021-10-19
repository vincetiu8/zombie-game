using Objects;
using UnityEngine;

namespace Enemy
{
	public class AnimatedCollisionDamager : CollisionDamager
	{
		private static readonly int AttackingProperty   = Animator.StringToHash("Attacking");
		private static readonly int AttackSpeedProperty = Animator.StringToHash("Attack Speed");

		private Animator _animator;

		protected void Start()
		{
			_animator = GetComponent<Animator>();
			_animator.SetFloat(AttackSpeedProperty, 0.5f / damageCooldown);
		}

		protected override void OnTriggerEnter2D(Collider2D other)
		{
			base.OnTriggerEnter2D(other);

			if (_animator == null) return;

			_animator.SetBool(AttackingProperty, HealthControllers.Count > 0);
		}

		protected override void OnTriggerExit2D(Collider2D other)
		{
			base.OnTriggerExit2D(other);

			if (_animator == null) return;

			_animator.SetBool(AttackingProperty, HealthControllers.Count > 0);
		}
	}
}
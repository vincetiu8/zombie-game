using UnityEngine;
using Utils;
using Weapons;

namespace Enemy
{
	public class AnimatedCollisionDamager : CollisionDamager
	{
		private static readonly int AttackingProperty   = Animator.StringToHash("Attacking");
		private static readonly int AttackSpeedProperty = Animator.StringToHash("Attack Speed");

		private Animator _animator;
		private int      _numPlayersInRange;


		protected override void Start()
		{
			base.Start();
			_animator = GetComponent<Animator>();
			_animator.SetFloat(AttackSpeedProperty, 0.5f / damageCooldown);
			_numPlayersInRange = 0;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			_animator.SetBool(AttackingProperty, true);
			_numPlayersInRange++;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			_numPlayersInRange--;
			_animator.SetBool(AttackingProperty, _numPlayersInRange > 0);
		}
	}
}
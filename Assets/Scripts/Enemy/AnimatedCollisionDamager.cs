using UnityEngine;
using Utils;
using Weapons;

namespace Enemy
{
	public class AnimatedCollisionDamager : CollisionDamager
	{
		private static readonly int Attacking = Animator.StringToHash("Attacking");

		private Animator _animator;
		private int      _numPlayersInRange;

		protected override void Start()
		{
			_animator = GetComponent<Animator>();
			_numPlayersInRange = 0;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			_animator.SetBool(Attacking, true);
			_numPlayersInRange++;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			_numPlayersInRange--;
			_animator.SetBool(Attacking, _numPlayersInRange > 0);
		}
	}
}
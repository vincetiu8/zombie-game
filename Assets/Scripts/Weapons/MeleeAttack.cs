using System.ComponentModel;
using Enemy;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Weapons
{
	/// <summary>
	///     A weapon that attacks within a
	///     small radius of the player
	/// </summary>
	public class MeleeAttack : Weapon
	{
		private static readonly int Attack = Animator.StringToHash("attack");

		[Description("The weapon's attributes")] [SerializeField]
		private WeaponAttributes[] levels;

		private Animator   _animator;
		private MeleePoint _meleePoint;

		protected override void Start()
		{
			base.Start();
			maxLevel = levels.Length;
			currentAttributes = levels[currentLevel];
			_meleePoint = GetComponentInChildren<MeleePoint>();
			_animator = GetComponentInChildren<Animator>();
		}

		protected override void Fire()
		{
			base.Fire();
			photonView.RPC("RpcMeleeAnimation", RpcTarget.All);

			foreach (Collider2D correctedEnemy in _meleePoint.GetEnemiesInCollider())
			{
				Debug.Log(correctedEnemy.name);
				Debug.Log(currentAttributes.damage);

				correctedEnemy.GetComponent<EnemyHealth>().ChangeHealth(-currentAttributes.damage);

				if (correctedEnemy.GetComponent<KnockbackController>() == null) continue;

				float angle = TransformUtils.Vector2ToDeg(correctedEnemy.transform.position - transform.position);
				correctedEnemy.transform.GetComponent<KnockbackController>()
				              .TakeKnockBack(angle, currentAttributes.knockback);
			}
		}

		[PunRPC]
		private void RpcMeleeAnimation()
		{
			_animator.SetTrigger(Attack);
		}
	}
}
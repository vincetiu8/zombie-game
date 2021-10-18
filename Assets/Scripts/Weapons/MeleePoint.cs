using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons
{
	/// <summary>
	/// Handles enemies coming into contact with
	/// the player's melee weapon
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class MeleePoint : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
		private List<Collider2D> _hitTargets;

		private void Awake()
		{
			_hitTargets = new List<Collider2D>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if ((targetLayer.value & (1 << collision.gameObject.layer)) > 0)
			{
				_hitTargets.Add(collision);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if ((targetLayer.value & (1 << collision.gameObject.layer)) > 0)
			{
				_hitTargets.Remove(collision);
			}
		}

		/// <summary>
		/// Returns a list of enemies obtained from hitEnemies
		/// with all null (dead) enemies removed
		/// </summary>
		public List<Collider2D> GetTargetsInCollider()
		{
			_hitTargets.RemoveAll(target => target == null);
			return _hitTargets.ToList();
		}
	}
}
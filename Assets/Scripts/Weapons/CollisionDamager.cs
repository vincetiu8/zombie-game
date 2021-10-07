using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Weapons
{
	/// <summary>
	///     Damages an object as a function of time
	/// </summary>
	public class CollisionDamager : MonoBehaviour
	{
		[Description("The damage per attack")] [Range(0, 1000)]
		public int damage;

		[Description("The cooldown between damage reductions")]
		public float damageCooldown;

		[Description("The layers the collision will affect")] [SerializeField]
		protected LayerMask layerMask;

		private int        _colliderCount;
		private float      _cooldown;
		private PhotonView _photonView;

		protected virtual void Start()
		{
			_photonView = GetComponentInParent<PhotonView>();
		}

		protected virtual void Update()
		{
			// Reduce cooldown by time
			if (_colliderCount > 0 && _cooldown > 0) _cooldown -= Time.deltaTime;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			_colliderCount++;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			_colliderCount--;

			if (_colliderCount > 0) return;

			_cooldown = damageCooldown / 3;
		}

		protected virtual void OnTriggerStay2D(Collider2D other)
		{
			if (!_photonView.IsMine) return;

			// Make sure cooldown is complete and the collision is in the layermask to deal damage
			if (_cooldown > 0 || !MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			other.gameObject.GetComponentInParent<HealthController>().ChangeHealth(-damage);
			_cooldown = damageCooldown;
		}
	}
}
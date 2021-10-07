using System.Collections.Generic;
using System.ComponentModel;
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

		private float _cooldown;

		protected List<HealthController> HealthControllers;

		private void Awake()
		{
			HealthControllers = new List<HealthController>();
		}

		protected virtual void Update()
		{
			if (HealthControllers.Count == 0) return;

			// Reduce cooldown by time
			if (_cooldown > 0)
			{
				_cooldown -= Time.deltaTime;
				return;
			}

			HealthControllers.RemoveAll(item => item == null);

			foreach (HealthController healthController in HealthControllers.ToArray())
				healthController.ChangeHealth(-damage);

			_cooldown = damageCooldown;
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			HealthController healthController = other.GetComponentInParent<HealthController>();

			if (healthController == null) return;

			if (HealthControllers.Count == 0) _cooldown = damageCooldown / 3;

			HealthControllers.Add(healthController);
		}

		protected virtual void OnTriggerExit2D(Collider2D other)
		{
			if (!MiscUtils.IsInLayerMask(layerMask, other.gameObject.layer)) return;

			HealthController healthController = other.GetComponentInParent<HealthController>();

			if (healthController == null) return;

			HealthControllers.Remove(healthController);
		}
	}
}
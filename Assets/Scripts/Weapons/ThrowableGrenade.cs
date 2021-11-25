using PlayerScripts;
using UnityEngine;

namespace Weapons
{
	// Lets the throwable grenade bypass reloading
	public class ThrowableGrenade : ProjectileLauncher
	{
		[Tooltip("How much to multiply throw force by when using alt-fire")] [SerializeField]
		private float altFireForceMultiplier;

		private void OnEnable()
		{
			// Handles instantiation of grenade
			if (ammoInventory == null) return;

			_bulletsInMagazine = ammoInventory.GetAmmo(ammoType);
		}

		public override void Setup(AmmoInventory inventory)
		{
			base.Setup(inventory);
			OnEnable();
		}

		protected override void FireBullet(float angle, bool alt)
		{
			ammoInventory.WithdrawAmmo(ammoType, 1);
			base.FireBullet(angle, alt);
		}

		protected override void AltFireAction(GameObject bulletClone, float angle)
		{
			SetBulletAttributes(bulletClone, angle, altFireForceMultiplier);
		}
	}
}
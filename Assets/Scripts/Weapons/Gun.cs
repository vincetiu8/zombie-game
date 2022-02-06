using System.Collections;
using System.ComponentModel;
using PlayerScripts;
using UnityEngine;

namespace Weapons
{
	/// <summary>
	/// Gun is a Weapon that fires bullets.
	/// </summary>
	public abstract class Gun : Weapon
	{
		[Description("The gun's ammo type")] [SerializeField]
		protected AmmoType ammoType;

		[Description("Holds the GunAttributes for the gun")] [SerializeField]
		protected GunAttributes[] weaponLevels;

		[Description("The transform the bullets will shoot from")] [SerializeField]
		protected Transform firepoint;

		private GunAttributes _currentGunAttributes;
		private Vector2       _gunOffsetAdjustment;
		private Coroutine     _reloadCoroutine;

		protected int BulletsInMagazine;

		protected override void Awake()
		{
			base.Awake();
			MaxLevel = weaponLevels.Length;
			_currentGunAttributes = weaponLevels[CurrentLevel];
			currentAttributes = _currentGunAttributes;
			BulletsInMagazine = _currentGunAttributes.magazineSize;
		}

		public override void Setup(AmmoInventory inventory)
		{
			base.Setup(inventory);
			CalculateGunOffsetAdjustment();
		}

		protected override void Fire()
		{
			if (BulletsInMagazine < 1) return;

			if (_reloadCoroutine != null)
			{
				StopCoroutine(_reloadCoroutine);
				_reloadCoroutine = null;
			}

			FireBullets();

			// Remove a bullet from the magazine
			BulletsInMagazine--;
			base.Fire();
		}

		protected override void AltFire()
		{
			if (BulletsInMagazine < 1) return;

			if (_reloadCoroutine != null)
			{
				StopCoroutine(_reloadCoroutine);
				_reloadCoroutine = null;
			}

			FireBulletsAlt();

			BulletsInMagazine--;
			base.AltFire();
		}

		protected virtual void FireBullets()
		{
			float angle = firepoint.rotation.eulerAngles.z;
			FireBullet(angle, false);
		}

		protected virtual void FireBulletsAlt()
		{
			float angle = firepoint.rotation.eulerAngles.z;
			FireBullet(angle, true);
		}

		protected abstract void FireBullet(float angle, bool alt);

		public override void Reload()
		{
			if (_reloadCoroutine != null) return;

			_reloadCoroutine = StartCoroutine(ReloadCoroutine());
		}

		// Once this coroutine finishes, the weapon is reloaded
		private IEnumerator ReloadCoroutine()
		{
			yield return new WaitForSeconds(_currentGunAttributes.reloadTime);

			// Withdraw bullets from the player's inventory
			int maxBulletsToRetrieve = _currentGunAttributes.magazineSize - BulletsInMagazine;
			int retrievedBullets = AmmoInventory.WithdrawAmmo(ammoType, maxBulletsToRetrieve);
			BulletsInMagazine += retrievedBullets;

			// Make sure to set _reloadCoroutine to null so the player can reload again after
			_reloadCoroutine = null;
		}

		public override void Upgrade()
		{
			_currentGunAttributes = weaponLevels[CurrentLevel];
			currentAttributes = _currentGunAttributes;
		}

		/// <summary>
		///     Calculates the vertical distance between the firepoint and the player pivot.
		///     Abstracted into method in case we add multiple weapon sprites in the future.
		///     That would require recalculation of the offset each time a sprite is loaded.
		/// </summary>
		private void CalculateGunOffsetAdjustment()
		{
			_gunOffsetAdjustment = -transform.localPosition;
			_gunOffsetAdjustment.y -= firepoint.localPosition.y;
		}

		public override void FaceMouse(float distance)
		{
			// Gets the adjustment angle that the weaponPivot needs the rotate
			// This lines up the weapon with the mouse properly
			float angle = Mathf.Atan2(_gunOffsetAdjustment.y, distance + _gunOffsetAdjustment.x) * Mathf.Rad2Deg;

			transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		public override string ToString()
		{
			return $"{weaponName} Level {CurrentLevel} Stats:\n{currentAttributes}";
		}
	}
}
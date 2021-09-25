using System.Collections;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
	/// <summary>
	///     Gun is a Weapon that fires bullets
	/// </summary>
	public class Gun : Weapon
	{
		#region Variables

		[Description("The gun's ammo type")] [SerializeField]
		private AmmoType ammoType;

		[Description("Holds the GunAttributes for the gun")] [SerializeField]
		protected GunAttributes[] weaponLevels;

		[Description("The transform the bullets will shoot from")] [SerializeField]
		private Transform firepoint;

		[Description("The bullet prefab to be instantiated")] [SerializeField]
		private GameObject bulletPrefab;

		// The current GunAttributes
		private GunAttributes _currentGunAttributes;

		// Current number of bullets in the magazine
		private int _bulletsInMagazine;

		// Coroutine representing the reload
		// If this is not null, the player is currently reloading
		private Coroutine _reloadCoroutine;

		// The adjustment necessary to correct the gun's rotation
		private float _gunOffsetAdjustment;

		#endregion

		#region Methods

		protected override void Start()
		{
			base.Start();
			maxLevel = weaponLevels.Length;
			_currentGunAttributes = weaponLevels[currentLevel];
			currentAttributes = _currentGunAttributes;
			_bulletsInMagazine = _currentGunAttributes.magazineSize;
			CalculateGunOffsetAdjustment();
		}

		protected override void Fire()
		{
			if (_bulletsInMagazine < 1) return;

			if (_reloadCoroutine != null) StopCoroutine(_reloadCoroutine);

			GameObject bulletClone =
				PhotonNetwork.Instantiate(bulletPrefab.name, firepoint.position, firepoint.rotation);

			// Set the bullet's attributes
			Vector2 bulletVelocity = firepoint.right * _currentGunAttributes.bulletSpeed;
			bulletClone.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
			bulletClone.GetComponent<Bullet>().damage = currentAttributes.damage;


			// Remove a bullet from the magazine
			_bulletsInMagazine--;
			base.Fire();
		}

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
			_bulletsInMagazine = ammoInventory.WithdrawAmmo(ammoType, _currentGunAttributes.magazineSize);

			// Make sure to set _reloadCoroutine to null so the player can reload again after
			_reloadCoroutine = null;
		}

		public override void Upgrade()
		{
			_currentGunAttributes = weaponLevels[currentLevel];
			currentAttributes = _currentGunAttributes;
		}

		// Calculates the vertical distance between the firepoint and the player pivot
		// Abstracted into method in case we add multiple weapon sprites in the future
		// That would require recalculation of the offset each time a sprite is loaded
		private void CalculateGunOffsetAdjustment()
		{
			_gunOffsetAdjustment = -firepoint.localPosition.y - transform.localPosition.y;
		}

		public override void FaceMouse(float distance)
		{
			// Gets the adjustment angle that the weaponPivot needs the rotate
			// This lines up the weapon with the mouse properly
			float angle = Mathf.Atan2(_gunOffsetAdjustment, distance) * Mathf.Rad2Deg;

			transform.parent.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		public override string ToString()
		{
			return $"{weaponName} Level {currentLevel} Stats:\n{currentAttributes}";
		}

		#endregion
	}
}
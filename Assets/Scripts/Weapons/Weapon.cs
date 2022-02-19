using System.ComponentModel;
using Photon.Pun;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Weapons
{
	/// <summary>
	///     A weapon is an object the player can use to attack enemies.
	///     Weapon is an abstract class that has methods the player's WeaponHandler calls to use the weapon.
	/// </summary>
	public abstract class Weapon : MonoBehaviourPun
	{
		public WeaponAttributes currentAttributes;
		public UnityEvent       onAmmoChanged;

		[Description("The weapon's name")] public string weaponName;

		private float _fireCooldown;
		private bool  _isFiring;

		protected AmmoInventory AmmoInventory;
		protected int           CurrentLevel;
		protected int           MaxLevel;

		// Should be overridden to set maxLevel
		protected virtual void Awake()
		{
			CurrentLevel = 0;
			MaxLevel = 0;
		}

		private void Update()
		{
			// Only calculate updates for local player
			if (!photonView.IsMine) return;

			// Decrease _fireCooldown accordingly
			if (_fireCooldown > 0)
			{
				_fireCooldown -= Time.deltaTime;
				return;
			}

			// Check whether we can fire here again
			if (currentAttributes.fullAuto && _isFiring) Fire();
		}

		public virtual void Setup(AmmoInventory inventory)
		{
			AmmoInventory = inventory;
		}

		/// <summary>
		///     Called when mouse is pressed or released
		/// </summary>
		/// <param name="toggle">Whether the mouse was pressed or released</param>
		public void ToggleFire(bool toggle)
		{
			if (!currentAttributes.fullAuto)
			{
				// Just fires when pressed for semi auto
				if (toggle && _fireCooldown <= 0) Fire();
				return;
			}

			// Toggles firing for full auto weapons
			_isFiring = toggle;
		}

		public void ToggleAltFire(bool toggle)
		{
			if (!currentAttributes.fullAuto)
			{
				// Just fires when pressed for semi auto
				if (toggle && _fireCooldown <= 0) AltFire();
				return;
			}

			// Toggles firing for full auto weapons
			_isFiring = toggle;
		}

		protected virtual void Fire()
		{
			_fireCooldown = currentAttributes.fireCooldown;
		}

		protected virtual void AltFire()
		{
			_fireCooldown = currentAttributes.fireCooldown;
		}

		public virtual void Reload()
		{
		}

		public virtual void Upgrade()
		{
			// Block upgrading the weapon if there are no more levels left to unlock
			if (CurrentLevel > MaxLevel - 2) return;

			CurrentLevel++;
		}

		/// <summary>
		///     Optional method to align weapon with cursor if needed.
		/// </summary>
		/// <param name="distance">The distance from the cursor to the object</param>
		public virtual void FaceMouse(float distance)
		{
		}
	}
}
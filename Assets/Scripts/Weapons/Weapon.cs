using System.Collections;
using System.ComponentModel;
using Photon.Pun;
using PlayerScripts;
using UnityEngine;

namespace Weapons
{
	/// <summary>
	///     A weapon is an object the player can use to attack enemies.
	///     Weapon is an abstract class that has methods the player's WeaponHandler calls to use the weapon.
	/// </summary>
	public abstract class Weapon : MonoBehaviourPun
	{
		[Description("The weapon's name")] [SerializeField]
		protected string weaponName;

		private float _fireCooldown;
		private bool isSwitching;

		private   bool             _isFiring;
		protected AmmoInventory    ammoInventory;
		public    WeaponAttributes currentAttributes;
		protected int              currentLevel;
		private   WeaponsHandler   _weaponsHandler;
		[SerializeField] private   GameObject       Player;

		protected int maxLevel;

		// Should be overridden to set maxLevel
		protected virtual void Start()
		{
			currentLevel = 0;
			maxLevel = 0;
			_weaponsHandler = Player.GetComponent<WeaponsHandler>();
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
			ammoInventory = inventory;
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

		protected virtual void Fire()
		{
			_fireCooldown = currentAttributes.fireCooldown;
		}

		public virtual void Reload()
		{
		}

		public virtual void Upgrade()
		{
			// Block upgrading the weapon if there are no more levels left to unlock
			if (currentLevel > maxLevel - 2) return;

			currentLevel++;
		}

		/// <summary>
		///     Optional method to align weapon with cursor if needed.
		/// </summary>
		/// <param name="distance">The distance from the cursor to the object</param>
		public virtual void FaceMouse(float distance)
		{
		}

		public IEnumerator WeaponSwitchingCooldown(int selectedIndex) {
			isSwitching = true;
			RaycastGun raycastGun = _weaponsHandler.availableWeapons[selectedIndex].GetComponent<RaycastGun>();
			Debug.Log("Next Weapon:\n" +
			          "Switching Cooldown: "+raycastGun.currentAttributes.switchingCooldown+"\n" +
			          "Name: "+raycastGun.currentAttributes.description);
			yield return new WaitForSeconds(raycastGun.currentAttributes.switchingCooldown);
			_weaponsHandler.ActivateCurrentWeapon();
			isSwitching = false;
		}
	}
}
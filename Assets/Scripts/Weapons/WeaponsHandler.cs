using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
	/// <summary>
	///     WeaponsHandler handles the usage of a player's weapons.
	/// </summary>
	[RequireComponent(typeof(AmmoInventory))]
	public class WeaponsHandler : MonoBehaviourPun
	{
		#region Variables

		[Description("The camera the player will see")] [SerializeField]
		private Camera playerCamera;

		[Description("The child object containing the player's sprite and weapons")] [SerializeField]
		private Transform playerSprite;

		// The player's AmmoInventory
		private AmmoInventory _ammoInventory;

		// The current weapon the player is using
		private Weapon _currentWeapon;

		#endregion

		#region Methods

		private void Start()
		{
			_currentWeapon = GetComponentInChildren<Gun>();
			_ammoInventory = GetComponent<AmmoInventory>();
			_currentWeapon.Setup(_ammoInventory);
		}

		public void PreventFire(bool preventFire)
		{
			_currentWeapon.CanAttack = !preventFire;
			transform.Find("PlayerObject").Find("WeaponPivot").gameObject.SetActive(!preventFire);
		}

		#endregion

		#region Input Actions

		public void FireAction(InputAction.CallbackContext context)
		{
			if (!photonView.IsMine) return;

			// When the mouse is pressed down, two actions are sent: started and performed
			// We'll use performed here to check for the press
			if (context.performed)
			{
				_currentWeapon.ToggleFire(true);
				return;
			}

			// Cancelled indicates the mouse was released
			// This is mainly to cancel
			if (context.canceled) _currentWeapon.ToggleFire(false);
		}

		public void ReloadAction(InputAction.CallbackContext context)
		{
			if (!photonView.IsMine) return;

			// Make sure this is only when the reload button is pressed
			if (!context.performed) return;

			_currentWeapon.Reload();
		}

		// Makes the player face the mouse
		public void FaceMouse(InputAction.CallbackContext context)
		{
			if (!photonView.IsMine) return;

			if (!context.performed) return;

			Vector2 mousePos = playerCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

			Vector2 direction = mousePos - (Vector2)playerSprite.position;

			float angle = Utils.Vector2ToDeg(direction);

			playerSprite.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

			// Allows the current weapon to be adjusted to face the mouse
			if (_currentWeapon != null) _currentWeapon.FaceMouse(direction.magnitude);
		}

		#endregion
	}
}
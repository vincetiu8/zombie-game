using System.ComponentModel;
using Networking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Weapons
{
	/// <summary>
	///     WeaponsHandler handles the usage of a player's weapons.
	/// </summary>
	[RequireComponent(typeof(AmmoInventory))]
	public class WeaponsHandler : MonoBehaviourPun, INetworkSerializeView
	{
		private const int MouseDistPrecision = 10;

		[Description("The camera the player will see")] [SerializeField]
		private Camera playerCamera;

		[Description("The child object containing the player's sprite and weapons")] [SerializeField]
		private Transform playerSprite;

		[Description("List of available weapons the player can cycle through")] [SerializeField]
		private GameObject[] availableWeapons;


		private AmmoInventory _ammoInventory;
		private Weapon        _currentWeapon;
		private int           _currentWeaponIndex;
		private float         _mouseDist;
		private bool          _preventFire;

		private void Start()
		{
			_ammoInventory = GetComponent<AmmoInventory>();
			for (int i = 0; i < availableWeapons.Length; i++)
			{
				availableWeapons[i].GetComponent<Weapon>().Setup(_ammoInventory);
				availableWeapons[i].SetActive(i == 0);
			}

			_currentWeapon = availableWeapons[0].GetComponent<Weapon>();
		}

		public bool Serialize(byte[] data, ref int offset)
		{
			float zRot = playerSprite.localRotation.eulerAngles.z;
			BitUtils.WriteFloat(data, zRot, BitUtils.Deg2Byte, 8, ref offset);

			if (_currentWeapon == null) return true;

			BitUtils.WriteFloat(data, _mouseDist, MouseDistPrecision, 8, ref offset);

			return true;
		}

		public void Deserialize(byte[] data, ref int offset)
		{
			float zRot = BitUtils.ReadFloat(data, BitUtils.Deg2Byte, 8, ref offset);
			playerSprite.localRotation = Quaternion.AngleAxis(zRot, Vector3.forward);

			if (_currentWeapon == null) return;

			float mouseDist = BitUtils.ReadFloat(data, MouseDistPrecision, 8, ref offset);
			_currentWeapon.FaceMouse(mouseDist);
		}

		public void FireAction(InputAction.CallbackContext context)
		{
			if (!photonView.IsMine || _preventFire) return;

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
			if (!photonView.IsMine || _preventFire) return;

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

			float angle = TransformUtils.Vector2ToDeg(direction);

			playerSprite.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

			// Allows the current weapon to be adjusted to face the mouse
			if (_currentWeapon == null) return;

			_mouseDist = direction.magnitude;
			_currentWeapon.FaceMouse(_mouseDist);
		}

		public void WeaponSwitchingScrollAction(InputAction.CallbackContext context)
		{
			if (!context.performed || _preventFire)
			{
				return;
			}

			int scrollDirection = (int)context.ReadValue<float>();
			int selectedWeaponIndex = _currentWeaponIndex + scrollDirection + availableWeapons.Length;
			selectedWeaponIndex %= availableWeapons.Length;
			SelectWeapon(selectedWeaponIndex);
		}

		public void WeaponSwitchingAction(InputAction.CallbackContext context)
		{
			if (!context.performed || _preventFire)
			{
				return;
			}

			int keypressed = (int)context.ReadValue<float>();
			int selectedWeaponIndex = keypressed - 1;
			SelectWeapon(selectedWeaponIndex);
		}

		private void SelectWeapon(int selectedIndex)
		{
			photonView.RPC("RPCSelectWeapon", RpcTarget.All, selectedIndex);
			_currentWeapon = availableWeapons[selectedIndex].GetComponent<Weapon>();
		}

		[PunRPC]
		private void RPCSelectWeapon(int selectedIndex)
		{
			availableWeapons[_currentWeaponIndex].SetActive(false);
			availableWeapons[selectedIndex].SetActive(true);
			_currentWeaponIndex = selectedIndex;
		}

		public void ToggleFireEnabled(bool preventFire)
		{
			_preventFire = !preventFire;
			_currentWeapon.gameObject.SetActive(!_preventFire);
		}
	}
}
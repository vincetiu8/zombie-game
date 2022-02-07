using Networking;
using Photon.Pun;
using PlayerScripts;
using UnityEngine;
using Weapons;

namespace Shop
{
	public class WeaponMachine : ItemShop
	{
		[Header("Weapon machine settings")] [SerializeField]
		private GameObject weaponPrefab;

		protected override bool CanBuy()
		{
			if (!base.CanBuy()) return false;
			WeaponsHandler weaponsHandler = GameManager.Instance.localPlayerInstance.GetComponent<WeaponsHandler>();
			return !weaponsHandler.CheckIfWeaponAlreadyExists(weaponPrefab);
		}

		protected override void OnPurchase()
		{
			GameObject boughtWeapon = PhotonNetwork.Instantiate(weaponPrefab.name,
			                                                    GameManager.Instance.localPlayerInstance.transform
			                                                               .position, Quaternion.identity);
			WeaponPickup weaponPickup = boughtWeapon.GetComponent<WeaponPickup>();
			weaponPickup.PickupWeapon();
		}
	}
}
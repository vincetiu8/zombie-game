using System;
using Networking;
using Photon.Pun;
using PlayerScripts;
using UnityEngine;
using Weapons;

namespace Shop
{
    public class WeaponMachine : ItemShop
    {
        [Header(("Weapon machine settings"))]
        [SerializeField] private GameObject weaponPrefab;

        protected override void OnPurchase()
        {
            // WeaponsHandler handler = GameManager.Instance.localPlayerInstance.GetComponent<WeaponsHandler>();
            GameObject boughtWeapon = PhotonNetwork.Instantiate(weaponPrefab.name, GameManager.Instance.localPlayerInstance.transform.position, Quaternion.identity);
            WeaponPickup weaponPickup = boughtWeapon.GetComponent<WeaponPickup>();
            boughtWeapon.SetActive(false);
            weaponPickup.PickupWeapon();
            
            //if (!handler.CheckForDuplicates(boughtWeapon))
            //{
                //weaponPickup.PickupWeapon();
                //return;
            //}
            //Destroy(boughtWeapon);
        }
    }
}

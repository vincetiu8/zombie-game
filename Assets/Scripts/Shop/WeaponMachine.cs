using System;
using Networking;
using PlayerScripts;
using UnityEngine;

namespace Shop
{
    public class WeaponMachine : ItemShop
    {
        [Header(("Weapon machine settings"))]
        [SerializeField] private GameObject weaponPrefab;
        
        protected override void OnPurchase()
        {
            WeaponsHandler handler = GameManager.Instance.localPlayerInstance.GetComponent<WeaponsHandler>();
            if (handler == null) return;
            handler.AddWeapon(weaponPrefab);
        }
    }
}

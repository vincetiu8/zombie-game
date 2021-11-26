using System;
using Networking;
using Photon.Pun;
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
            GameObject player = handler.gameObject;
            GameObject boughtWeapon = PhotonNetwork.Instantiate(weaponPrefab.name, player.transform.position, Quaternion.identity);
            
            boughtWeapon.transform.parent = player.transform.Find("PlayerObject").Find("Weapons");
            boughtWeapon.GetComponent<Collider2D>().enabled = false;
            
            foreach (SpriteRenderer spriteRenderer in boughtWeapon.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("Actors");
                spriteRenderer.sortingOrder = 2;
            }
            
            boughtWeapon.SetActive(false);
            handler.AddWeapon(weaponPrefab);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;
using Shop;
using Interact;
using Photon.Pun;

public class AmmoMachine : Interactable
{
    [SerializeField] private AmmoType ammoType;
    private GoldSystem goldSystem;

    private void Start()
    {
        goldSystem = GameManager.instance.GetComponent<GoldSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteract>().AddInteractableObject(gameObject); 
        }
    }

    public override void Interact(GameObject player)
    {
        AmmoInventory ammoInventory = player.gameObject.GetComponent<AmmoInventory>();
        goldSystem.AddGold(new List<string> {PhotonNetwork.NickName}, 10);
        if(ammoInventory == null)
        {
            return;
        }
        ammoInventory.DepositAmmo(ammoType, 10);
        goldSystem.WithdrawGold(PhotonNetwork.NickName, 5);
        Debug.Log(goldSystem.GetPlayerGold(PhotonNetwork.NickName));
    }
}

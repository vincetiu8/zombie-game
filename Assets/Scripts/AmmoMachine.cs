using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;
using Shop;
using Interact;
using Photon.Pun;
using UnityEngine.UI;
using Menus_UI;
using UnityEngine.InputSystem;

public class AmmoMachine : Interactable
{

    #region private variables

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private Text ammoTypeText;
    [SerializeField] private GameObject ammoMachineUI;
    private GoldSystem _goldSystem;
    private MenuManager _menuManager;

    #endregion


    #region private methods

    private void Start()
    {
        _goldSystem = GameManager.instance.GetComponent<GoldSystem>();
        _menuManager = MenuManager.Instance.GetComponent<MenuManager>();
        ammoTypeText.text = ammoType.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteract>().AddInteractableObject(gameObject);
        }
    }

    #endregion


    #region public methods

    public void PurchaseAmmo()
    {
        GameObject customer = GameManager.instance.player;
        //for testing
        _goldSystem.AddGold(new List<string> {PhotonNetwork.NickName}, 10);

        AmmoInventory ammoInventory = customer.gameObject.GetComponent<AmmoInventory>();
        if(ammoInventory == null)
        {
            return;
        }
        ammoInventory.DepositAmmo(ammoType, 10);
        _goldSystem.WithdrawGold(PhotonNetwork.NickName, 5);

        Debug.Log(_goldSystem.GetPlayerGold(PhotonNetwork.NickName));
    }

    public override void Interact(GameObject player)
    {
        _menuManager.OpenMenu("ammoMachine");
    }

    #endregion
}
